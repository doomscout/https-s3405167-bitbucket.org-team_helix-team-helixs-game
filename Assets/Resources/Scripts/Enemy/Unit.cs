using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : Entity{
	List<Direction> list_directions = new List<Direction>();
	SimpleAI brain;

	Direction Alignment = Direction.None;

	private List<float> list_of_damage_taken;
	private List<Colour> list_of_colour_taken;

	private int id = 0;

	public Unit() : base(){
		brain = new SimpleAI(this);


		list_of_damage_taken = new List<float>();
		list_of_colour_taken = new List<Colour>();
	}

	public Unit(int i) : this(){
		id = i;
		game_object.name = i + "";
	}

	protected override void InitMapPosition() {
		Map_position_x = Random.Range(0, GameTools.Map.size_x);
		Map_position_y = Random.Range(0, GameTools.Map.size_z);

		while (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] != null || 
		       (!TileTools.IsLand(GameTools.Map.TileMapData[Map_position_x, Map_position_y])) ||
		       GraphSearch.fromPosition(Map_position_x, Map_position_y).manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) < 10) {
			Map_position_x = Random.Range(0, GameTools.Map.size_x);
			Map_position_y = Random.Range(0, GameTools.Map.size_z);
		}
	}
	
	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/EnemyPrefab", typeof(GameObject))) as GameObject;
		}
		base.game_object.transform.position = new Vector3(Map_position_x, 0, Map_position_y);
		game_object.renderer.material.color = ColourManager.toColor(MainColour);
		GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = this;
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
	
	public override bool animation_tick() {
		if (!base.animation_tick()) {
			FinishedAnimation = true;
			return true;
		}
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
			break;
		case Direction.Down:
			game_object.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
			break;
		case Direction.Left:
			game_object.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
			break;
		case Direction.Right:
			game_object.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
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
			
			current_target = Direction.None;
		}
		return false;
	}

	public override float GetHitByMagic(Spell taken_spell) {
		float dmg = base.GetHitByMagic(taken_spell);
		list_of_damage_taken.Add(dmg);
		list_of_colour_taken.Add(taken_spell.SpellColour);
		brain.IsHit = true;
		Debug.Log ("Got hit");
		return dmg;
	}

	/* Maybe make the unit search for a valid target before shooting, as opposed to always shooting at the player */
	public override void CastMainSpell() {
		base.CastMainSpell();
		Alignment = Direction.None;
		/* new animation */
		ProjectileManager.getInstance().queueProjectile(MainSpell, game_object.transform.position, GameTools.Player.game_object.transform.position);
		MainSpell.loadInfo(	new int[2]{ Map_position_x, Map_position_y},
							new int[2] {GameTools.Player.Map_position_x, GameTools.Player.Map_position_y});
		game_object.transform.LookAt(new Vector3(GameTools.Player.Map_position_x, 0, GameTools.Player.Map_position_y));
	}
	
	public void showDamageTakenAnimation() {
		for (int i = 0; i < list_of_damage_taken.Count; i++) {
			Color c = Color.white;
			if (ColourManager.getWeakness(list_of_colour_taken[i]) == MainColour) {
				c = Color.magenta;
			}
			base.ShowText(list_of_damage_taken[i] + "", c, i);
		}
		list_of_damage_taken = new List<float>();
		list_of_colour_taken = new List<Colour>();
	}

	public Vector2 GroupCenterOfMass(List<Entity> neighbourhood) {
		if (neighbourhood.Count == 0) {
			return new Vector2(0,0);
		}
		float avgX = 0;
		float avgY = 0;
		for (int i = 0; i < neighbourhood.Count; i++) {
			avgX += neighbourhood[i].Map_position_x - Map_position_x;
			avgY += neighbourhood[i].Map_position_y - Map_position_y;
		}
		avgX /= neighbourhood.Count;
		avgY /= neighbourhood.Count;

		return new Vector2(avgX, avgY);
	}

	public Vector2 NeighbourhoodAlignment(List<Entity> neighbourhood) {
		Vector2 v = new Vector2(0,0);
		if (neighbourhood.Count == 0) {
			return new Vector2(0,0);
		}
		for (int i = 0; i < neighbourhood.Count; i++) {
			Direction otherD = ((Unit)neighbourhood[i]).Alignment;
			if (otherD != Direction.None) {
				Debug.Log ("In herererere");
				switch(otherD) {
				case Direction.Down:
					v += new Vector2(0, -1);
					break;
				case Direction.Up:
					v += new Vector2(0, 1);
					break;
				case Direction.Right:
					v += new Vector2(1, 0);
					break;
				case Direction.Left:
					v += new Vector2(-1, 0);
					break;
				}
			}
		}
		return v;
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
		goal.Normalize();

		Heap<DirectionWeight> orderedList = new Heap<DirectionWeight>(new DirectionWeightComparer());
		Direction calculatedDirection = Direction.None;

		if (goal.y >= 0) {
			orderedList.insert(new DirectionWeight(Direction.Up, ((int)(1.0f - Mathf.Abs(goal.y)) * 10000), 0, 0, 0, 0));
			orderedList.insert(new DirectionWeight(Direction.Down, ((int)(Mathf.Abs(goal.y)) * 10000), 0, 0, 0, 0));
		} else {
			orderedList.insert(new DirectionWeight(Direction.Down, ((int)(1.0 - Mathf.Abs(goal.y)) * 10000), 0, 0, 0, 0));
			orderedList.insert(new DirectionWeight(Direction.Up, ((int)(Mathf.Abs(goal.y)) * 10000), 0, 0, 0, 0));

		}
		if (goal.x >= 0) {
			orderedList.insert(new DirectionWeight(Direction.Right, ((int)(1.0f - Mathf.Abs(goal.x)) * 10000), 0, 0, 0, 0));
			orderedList.insert(new DirectionWeight(Direction.Left, ((int)(Mathf.Abs(goal.x)) * 10000), 0, 0, 0, 0));
		} else {
			orderedList.insert(new DirectionWeight(Direction.Left, ((int)(1.0f - Mathf.Abs(goal.x)) * 100), 0, 0, 0, 0));
			orderedList.insert(new DirectionWeight(Direction.Right, ((int)(Mathf.Abs(goal.x)) * 10000), 0, 0, 0, 0));
		}	


		bool validMove = false;
		
		int orderedCount = orderedList.length();
		int newX = 0, newY = 0;
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
		int currVal = GameTools.Map.WeightedMap[Map_position_x, Map_position_y];
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
				Alignment = d;
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
