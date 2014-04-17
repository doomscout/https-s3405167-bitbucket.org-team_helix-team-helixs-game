using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {None, Up, Down, Left, Right};

public class Unit {
	public bool FinishedAnimation{get;set;}
	//public bool IsDead{get; private set;}
	public float MoveSpeed{get;set;}
	public float Health{get;private set;}
	GameObject unit;

	List<Direction> list_directions = new List<Direction>();
	Direction current_target = Direction.None;
	SimpleAI brain;
	float remainingDistance = 1f;

	public Unit() {
		brain = new SimpleAI(this);
		unit = Object.Instantiate(Resources.Load("Prefabs/EnemyPrefab", typeof(GameObject))) as GameObject;
        unit.transform.position = new Vector3(Random.Range(2, 10), 0, Random.Range(2, 10));
		Health = 10f;
		MoveSpeed = 10.0f;
	}

	public void determineNextMove() {
		list_directions = new List<Direction>();
		list_directions.Add (Direction.Up);
	}	

	public void animation_tick() {
		if (Health <= 0) {
			//IsDead = true;
			FinishedAnimation = true;
			GameTools.TM.signalDeath(this);
			return;
		}
		if (current_target == Direction.None) {
			if (list_directions.Count > 0) {
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
				unit.transform.Translate(0, 0, MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Down:
				unit.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Left:
				unit.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			case Direction.Right:
				unit.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance < 0) {
			//We've arrived at our destination, but overshot a little bit
			//correct overshooting
			//unit.transform.position.x = Mathf.Round(unit.transform.position.x);
			//unit.transform.position.z = Mathf.Round(unit.transform.position.z);
			current_target = Direction.None;
		}

	}

	public void death_tick() {
		//display death animation (if any)
		FinishedAnimation = true;			//temp no animation, just return immediately
		Debug.Log ("Death Tick");
	}

	public void addMove(Direction d) {
		list_directions.Add(d);
	}


}
