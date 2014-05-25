using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : Entity{
	List<Direction> list_directions = new List<Direction>();
	SimpleAI brain;
	public bool IsAggroed = false;

	private Animator enemyAnimation;
	private int possibleX = 0;
	private int possibleY = 0;
	
	public Unit() : base(){
		brain = new SimpleAI(this);
	}

	public Unit(int i) : this(){
		game_object.name = i + "";
	}

	public Unit(int x, int y) : base(x,y) {
		brain = new SimpleAI(this);
		Map_position_x = x;
		Map_position_y = y;
	}

	protected override void InitMapPosition() {
		if (Map_position_x == 0 && Map_position_y == 0) {
			Map_position_x = Random.Range(0, GameTools.Map.size_x);
			Map_position_y = Random.Range(0, GameTools.Map.size_z);

			while (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] != null || 
			       (!TileTools.IsLand(GameTools.Map.TileMapData[Map_position_x, Map_position_y])) ||
			       GraphSearch.fromPosition(Map_position_x, Map_position_y).manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) < 10) {
				Map_position_x = Random.Range(0, GameTools.Map.size_x);
				Map_position_y = Random.Range(0, GameTools.Map.size_z);
			}
		}
	}
	
	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/enemy", typeof(GameObject))) as GameObject;
		}
		base.game_object.transform.position = new Vector3(Map_position_x, 0.1f, Map_position_y);
		game_object.renderer.material.color = ColourManager.toColor(MainColour);
		GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = this;
		enemyAnimation = game_object.GetComponent<Animator> ();
	}

	protected override void InitCleanable () {
		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	public void logic_tick() {
		if (!IsDead()) {
			brain.tick();
		}
	}

	public override bool IsDead() {
		bool isDead = base.IsDead();
		if (isDead) {
			GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
		}
		return isDead;
	}

	public override void death_tick () {
		enemyAnimation.SetBool ("Death", true);
		base.death_tick();
		GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
	}
	
	public override bool animation_tick() {
		if (!base.animation_tick()) {
			FinishedAnimation = true;
			return true;
		}
		enemyAnimation.SetBool ("Cast", false);
		if (current_target == Direction.None) {
			if (list_directions.Count > 0) {
				if (list_directions[0] == Direction.None) {
					FinishedAnimation = true;
					return true;
				}
				current_target = list_directions[0];
				list_directions.RemoveAt(0);
				remainingDistance = 1.0f;
				FinishedAnimation = false;
			} else {
				//there's no current target and there's no directions in the directions list
				FinishedAnimation = true;
				return true;
			}
		}
		
		switch (current_target) {
		case Direction.Up:
			game_object.transform.Translate(0, 0, MoveSpeed * Time.deltaTime, null);
			enemyAnimation.SetInteger("Direction", 0);
			break;
		case Direction.Down:
			game_object.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
			enemyAnimation.SetInteger("Direction", 1);
			break;
		case Direction.Left:
			game_object.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
			enemyAnimation.SetInteger("Direction", 2);
			break;
		case Direction.Right:
			game_object.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
			enemyAnimation.SetInteger("Direction", 3);
			break;
		default:
			Debug.Log ("Defaulted");
			break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance < 0) {
			//We've arrived at our destination, but overshot a little bit
			//correct overshooting
			Vector3 temp = game_object.transform.position;
			game_object.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));
			enemyAnimation.SetInteger("Direction", 4);
			current_target = Direction.None;
		}
		return false;
	}

	public override float GetHitByMagic(Spell taken_spell) {
		float dmg = base.GetHitByMagic(taken_spell);
		Health -= dmg;
		base.ShowText("-" + dmg, Color.black, 0);
		BattleLog.GetInstance().AddMessage("[Turn " + GameTools.GI.NumberOfTurnsUntilWin +"] Unit took " + dmg + " damage.");
		IsAggroed = true;
		return dmg;
	}

	/* Maybe make the unit search for a valid target before shooting, as opposed to always shooting at the player */
	public override void CastMainSpell() {
		base.CastMainSpell();
		/* new animation */

		if (GraphSearch
		    .fromPosition(Map_position_x, Map_position_y)
		    .manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) <= MainSpell.CastRange) {
			MainSpell.loadInfo(	new int[2]{ Map_position_x, Map_position_y},
								new int[2] {GameTools.Player.Map_position_x, GameTools.Player.Map_position_y});
			ProjectileManager.getInstance().queueProjectile(MainSpell, game_object.transform.position, GameTools.Player.game_object.transform.position);
		} else if (GraphSearch
		           .fromPosition(Map_position_x, Map_position_y)
		           .manhattanDistanceFromTarget(GameTools.Base.Map_position_x, GameTools.Base.Map_position_y) <= MainSpell.CastRange){
			MainSpell.loadInfo(	new int[2]{ Map_position_x, Map_position_y},
								new int[2] {GameTools.Base.Map_position_x, GameTools.Base.Map_position_y});
			ProjectileManager.getInstance().queueProjectile(MainSpell, game_object.transform.position, GameTools.Base.game_object.transform.position);
		} else {
			Debug.LogError("Not in either range");
		}

		enemyAnimation.SetBool ("Cast", true);

	}

	public Vector2 GroupCenterOfMass(List<Entity> neighbourhood) {
		if (neighbourhood.Count == 0) {
			return new Vector2(0,0);
		}
		float avgX = 0;
		float avgY = 0;
		for (int i = 0; i < neighbourhood.Count; i++) {
			avgX += neighbourhood[i].Map_position_x;
			avgY += neighbourhood[i].Map_position_y;
		}
		avgX /= neighbourhood.Count;
		avgY /= neighbourhood.Count;

		return new Vector2(avgX, avgY);
	}

	public void MoveWithNeighbours(List<Entity> neighbourhood) {
		if (neighbourhood.Count == 0) {
			return;
		}
		list_directions = new List<Direction>();

		Vector2 goal = GroupCenterOfMass(neighbourhood);
		if (goal.x == 0 && goal.y == 0) {
			return;
		}
		Vector2 goalDir = new Vector2(goal.x - Map_position_x, goal.y - Map_position_y);
		goalDir.Normalize();

		Heap<DirectionWeight> orderedList = new Heap<DirectionWeight>(new DirectionWeightComparer());

		int newX = 0, newY = 0;
		
		if (goalDir.y >= 0) {
			newX = Map_position_x;
			newY = Map_position_y + 1;
			orderedList.insert(new DirectionWeight(Direction.Up, ((int)((-Mathf.Abs(goalDir.y)) * 10)), 
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
			newX = Map_position_x;
			newY = Map_position_y - 1;
			orderedList.insert(new DirectionWeight(Direction.Down, ((int)(Mathf.Abs(goalDir.y)) * 10),
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
		} else {
			newX = Map_position_x;
			newY = Map_position_y - 1;
			orderedList.insert(new DirectionWeight(Direction.Down, ((int)((-Mathf.Abs(goalDir.y)) * 10)),
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
			newX = Map_position_x;
			newY = Map_position_y + 1;
			orderedList.insert(new DirectionWeight(Direction.Up, ((int)(Mathf.Abs(goalDir.y)) * 10), 
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
		}
		if (goalDir.x >= 0) {
			newX = Map_position_x + 1;
			newY = Map_position_y;
			orderedList.insert(new DirectionWeight(Direction.Right, ((int)((-Mathf.Abs(goalDir.x)) * 10)), 
			                                      	newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
			newX = Map_position_x - 1;
			newY = Map_position_y;
			orderedList.insert(new DirectionWeight(Direction.Left, ((int)(Mathf.Abs(goalDir.x)) * 10), 
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
		} else {
			newX = Map_position_x - 1;
			newY = Map_position_y;
			orderedList.insert(new DirectionWeight(Direction.Left, ((int)((-Mathf.Abs(goalDir.x)) * 10)),
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
			newX = Map_position_x + 1;
			newY = Map_position_y;
			orderedList.insert(new DirectionWeight(Direction.Right, ((int)(Mathf.Abs(goalDir.x)) * 10),
			                                       newX, newY, Mathf.RoundToInt(goal.x), Mathf.RoundToInt(goal.y)));
		}	


		bool validMove = false;
		
		int orderedCount = orderedList.length();

		for (int i = 0; i < orderedCount; i++) {
			Direction d = orderedList.extract().d;
			switch (d) {
			case Direction.Right:
				newX = Map_position_x + 1;
				newY = Map_position_y;
				break;
			case Direction.Left:
				newX = Map_position_x - 1;
				newY = Map_position_y;
				break;
			case Direction.Up:
				newX = Map_position_x;
				newY = Map_position_y + 1;
				break;
			case Direction.Down:
				newX = Map_position_x;
				newY = Map_position_y - 1;
				break;
			}
			if (GameTools.Map.WeightedMap[newX, newY] != 0 &&
				GameTools.Map.map_unit_occupy[newX, newY] == null &&
			    (GameTools.Player.Map_position_x != newX || GameTools.Player.Map_position_y != newY) ) {
				validMove = true;
				list_directions.Add(d);
				break;
			}
		}
		
		//Occupy the new position (it might be the same one thanks to old_x and old_y)
		if (validMove) {
			GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
			GameTools.Map.map_unit_occupy[newX, newY] = this;
			Map_position_x = newX;
			Map_position_y = newY;
		}
	}

	public void MoveRandomly() {
		int newX = 0, newY = 0;
		for (int i = 0; i < 10; i++) {
			Direction d = (Direction)Random.Range(1, 5);
			switch (d) {
			case Direction.Right:
				newX = Map_position_x + 1;
				newY = Map_position_y;
				break;
			case Direction.Left:
				newX = Map_position_x - 1;
				newY = Map_position_y;
				break;
			case Direction.Up:
				newX = Map_position_x;
				newY = Map_position_y + 1;
				break;
			case Direction.Down:
				newX = Map_position_x;
				newY = Map_position_y - 1;
				break;
			default:
				Debug.LogError("Defaulted");
				break;
			}
			if (GameTools.Map.map_unit_occupy[newX, newY] == null &&
			    (GameTools.Player.Map_position_x != newX || GameTools.Player.Map_position_y != newY)) {
				list_directions.Add(d);
				break;
			}
		}
	}

	public void determineNextMove() {
		list_directions = new List<Direction>();
		Heap<DirectionWeight> orderedList = new Heap<DirectionWeight>(new DirectionWeightComparer());

		int newX, newY;
		newX = Map_position_x + 1;
		newY = Map_position_y;
		if ((!MapTools.IsOutOfBounds(newX, newY)) &&
		    GameTools.Map.WeightedMap[newX, newY] != 0) {
			orderedList.insert(new DirectionWeight(Direction.Right, GameTools.Map.WeightedMap[newX, newY], newX, newY, 
			                                       GameTools.Player.Map_position_x, GameTools.Player.Map_position_y));
		}
		newX = Map_position_x - 1;
		newY = Map_position_y;
		if ((!MapTools.IsOutOfBounds(newX, newY)) &&
		    GameTools.Map.WeightedMap[newX, newY] != 0) {
			orderedList.insert(new DirectionWeight(Direction.Left, GameTools.Map.WeightedMap[newX, newY], newX, newY, 
			                                       GameTools.Player.Map_position_x, GameTools.Player.Map_position_y));
		}
		newX = Map_position_x;
		newY = Map_position_y + 1;
		if ((!MapTools.IsOutOfBounds(newX, newY)) &&
		    GameTools.Map.WeightedMap[newX, newY] != 0) {
			orderedList.insert(new DirectionWeight(Direction.Up, GameTools.Map.WeightedMap[newX, newY], newX, newY, 
			                                        GameTools.Player.Map_position_x, GameTools.Player.Map_position_y));
		}
		newX = Map_position_x;
		newY = Map_position_y - 1;
		if ((!MapTools.IsOutOfBounds(newX, newY)) &&
		    GameTools.Map.WeightedMap[newX, newY] != 0) {
			orderedList.insert(new DirectionWeight(Direction.Down, GameTools.Map.WeightedMap[newX, newY], newX, newY, 
			                                       GameTools.Player.Map_position_x, GameTools.Player.Map_position_y));
		}


		bool validMove = false;

		int orderedCount = orderedList.length();
		for (int i = 0; i < orderedCount; i++) {
			Direction d = orderedList.extract().d;
			switch (d) {
			case Direction.Right:
				newX = Map_position_x + 1;
				newY = Map_position_y;
				break;
			case Direction.Left:
				newX = Map_position_x - 1;
				newY = Map_position_y;
				break;
			case Direction.Up:
				newX = Map_position_x;
				newY = Map_position_y + 1;
				break;
			case Direction.Down:
				newX = Map_position_x;
				newY = Map_position_y - 1;
				break;
			}
			if (GameTools.Map.map_unit_occupy[newX, newY] == null &&
			    (GameTools.Player.Map_position_x != newX || GameTools.Player.Map_position_y != newY)) {
				validMove = true;
				list_directions.Add(d);
				break;
			}
		}

		//Occupy the new position (it might be the same one thanks to old_x and old_y)
		if (validMove) {
			GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
			GameTools.Map.map_unit_occupy[newX, newY] = this;
			Map_position_x = newX;
			Map_position_y = newY;
		}
	
	}

	public override void OnClickAction() {		
		GameTools.GI.ToggleUnitIndicator(this);
	}
	
}
