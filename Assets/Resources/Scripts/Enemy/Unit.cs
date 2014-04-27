﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit {
	public bool FinishedAnimation{get;set;}
	//public bool IsDead{get; private set;}
	public float MoveSpeed{get;set;}
	public float Health{get;private set;}
    public int Map_position_x { get; private set;}
    public int Map_position_y { get; private set;}
	public Colour UnitColour {get; set;}
	GameObject unit_object;

	List<Direction> list_directions = new List<Direction>();
	Direction current_target = Direction.None;
	SimpleAI brain;
	float remainingDistance = 1f;

	//AStar pathFinder;


	public Unit() {
		brain = new SimpleAI(this);
		unit_object = Object.Instantiate(Resources.Load("Prefabs/EnemyPrefab", typeof(GameObject))) as GameObject;
		Map_position_x = Random.Range(0, GameTools.Map.size_x);
		Map_position_y = Random.Range(0, GameTools.Map.size_z);

		while (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] != null || 
		       GameTools.Map.store_data[Map_position_x, Map_position_y] == Colour.None) {
			Map_position_x = Random.Range(0, GameTools.Map.size_x);
			Map_position_y = Random.Range(0, GameTools.Map.size_z);
		}

		GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = this;
        unit_object.transform.position = new Vector3(Map_position_x, 0, Map_position_y);
		Health = 10f;
		MoveSpeed = 10.0f;
		UnitColour = ColourManager.getRandomColour();

		//pathFinder = new AStar();
	}

	public void logic_tick() {
		brain.tick();
	}
	
	public void animation_tick() {
		if (Health <= 0) {
			//IsDead = true;
			FinishedAnimation = true;
			GameTools.TM.signalDeath(this);
			GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = null;
			return;
		}
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
			unit_object.transform.Translate(0, 0, MoveSpeed * Time.deltaTime, null);
			break;
		case Direction.Down:
			unit_object.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
			break;
		case Direction.Left:
			unit_object.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
			break;
		case Direction.Right:
			unit_object.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
			break;
		default:
			Debug.Log ("Defaulted");
			break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance < 0) {
			//We've arrived at our destination, but overshot a little bit
			//correct overshooting
			Vector3 temp = unit_object.transform.position;
			unit_object.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));
			
			current_target = Direction.None;
		}
		
	}
	
	public void death_tick() {
		//display death animation (if any)
		GameObject.Destroy(unit_object);
		FinishedAnimation = true;			//temp no animation, just return immediately
		Debug.Log ("Death Tick");
	}

	public void takeDamage(Spell taken_spell) {
		float modifier = 1.0f;
		if (ColourManager.getWeakness(taken_spell.SpellColour) == UnitColour) {
			//The spell is weak against our colour
			modifier = ColourManager.WeaknessModifier;
		} else if (ColourManager.getStrength(taken_spell.SpellColour) == UnitColour) {
			//The spell is strong against us
			modifier = ColourManager.StrengthModifier;
		}
		Health -= taken_spell.Power * modifier;
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



}
