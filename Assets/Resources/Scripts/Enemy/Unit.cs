using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : Entity{
	List<Direction> list_directions = new List<Direction>();
	SimpleAI brain;

	private List<float> list_of_damage_taken;
	private List<Colour> list_of_colour_taken;
	
	public Unit() {
		brain = new SimpleAI(this);

		list_of_damage_taken = new List<float>();
		list_of_colour_taken = new List<Colour>();
	}

	protected override void InitMapPosition() {
		Map_position_x = Random.Range(0, GameTools.Map.size_x);
		Map_position_y = Random.Range(0, GameTools.Map.size_z);

		while (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] != null || 
		       GameTools.Map.store_data[Map_position_x, Map_position_y] == Colour.None ||
		       AStar.fromPosition(Map_position_x, Map_position_y).manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) < 10) {
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

	public new bool IsDead() {
		bool isDead = base.IsDead();
		if (isDead) {
			GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
		}
		return isDead;
	}
	
	public override void animation_tick() {
		if (current_target == Direction.None) {
			if (list_directions.Count > 0) {
				if (list_directions[0] == Direction.None) {
					FinishedAnimation = true;
					return;
				}
				current_target = list_directions[0];
				list_directions.RemoveAt(0);
				remainingDistance = 1.0f;
				FinishedAnimation = false;
			} else {
				//there's no current target and there's no directions in the directions list
				FinishedAnimation = true;
				return;
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
	}

	public new float GetHitByMagic(Spell taken_spell) {
		float dmg = base.GetHitByMagic(taken_spell);
		list_of_damage_taken.Add(dmg);
		list_of_colour_taken.Add(taken_spell.SpellColour);
		return dmg;
	}

	/* Maybe make the unit search for a valid target before shooting, as opposed to always shooting at the player */
	public new void CastMainSpell() {
		base.CastMainSpell();
		/* new animation */
		ProjectileManager.getInstance().queueProjectile(MainSpell, game_object.transform.position, GameTools.Player.game_object.transform.position);
		MainSpell.loadInfo(	new int[2]{ Map_position_x, Map_position_y},
							new int[2] {GameTools.Player.Map_position_x, GameTools.Player.Map_position_y});
		game_object.transform.LookAt(new Vector3(GameTools.Player.Map_position_x, 0, GameTools.Player.Map_position_y));
	}
	
	public void showDamageTakenAnimation() {
		for (int i = 0; i < list_of_damage_taken.Count; i++) {
			GameObject o = Object.Instantiate(Resources.Load("Prefabs/DamagePopupPrefab", typeof(GameObject))) as GameObject;
			DamagePopup script = o.GetComponent<DamagePopup>();
			Color c = Color.white;
			if (ColourManager.getWeakness(list_of_colour_taken[i]) == MainColour) {
				c = Color.magenta;
			}
			script.setText(list_of_damage_taken[i] + "");
			script.setColor(c);
			o.transform.position = new Vector3(game_object.transform.position.x, 0, game_object.transform.position.z + 1.0f + i/2.0f);

		}
		list_of_damage_taken = new List<float>();
		list_of_colour_taken = new List<Colour>();
	}

	public void determineNextMove() {
		Stack<Direction> stackOfDirections = AStar
												.fromPosition(Map_position_x, Map_position_y)
												.findPathToPostion(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y);
		list_directions = new List<Direction>();
		if (stackOfDirections.Count == 0) {
			list_directions.Add(Direction.None);
			return;
		}
		Direction d = stackOfDirections.Pop();
		if (d == Direction.None) {
			if (stackOfDirections.Count == 0) {
				//Can't move anywhere, let's not move
				list_directions.Add(Direction.None);
				return;
			}
			d = stackOfDirections.Pop ();
			if (d == Direction.None) {
				Debug.LogError("Two Nones in a row, something's wrong");
			} else {	//We can loop over the whole path here, if we want
				int old_x = Map_position_x, old_y = Map_position_y;
				//We're moving, so un-occupy our current position
				GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
				list_directions.Add (d);

				switch (d) {
				case Direction.Up:
					Map_position_y++;
					break;
				case Direction.Down:
					Map_position_y--;
					break;
				case Direction.Left: 
					Map_position_x--;
					break;
				case Direction.Right:
					Map_position_x++;
					break;
				}

				//Don't let the enemy move on top of the player
				if (Map_position_x == GameTools.Player.Map_position_x && Map_position_y == GameTools.Player.Map_position_y) {
					Map_position_x = old_x;
					Map_position_y = old_y;
					list_directions.Remove(d);
				}
				//Don't let the enemy move on top of each other
				if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y]  != null) {
					Map_position_x = old_x;
					Map_position_y = old_y;
					list_directions.Remove(d);
				}
				//Occupy the new position (it might be the same one thanks to old_x and old_y)
				GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = this;
			}
		} else {
			Debug.LogError("No initial none direction, something's wrong");
		}
	}

	public void OnClickAction() {		
		GameTools.GI.UnitCastIndicator.ToggleUnit(this);
	}


}
