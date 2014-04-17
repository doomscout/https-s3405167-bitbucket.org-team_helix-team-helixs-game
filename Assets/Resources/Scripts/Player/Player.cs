using UnityEngine;
using System.Collections;

public class Player {
	public bool FinishedAnimation{get;set;}
	public bool IsDead{get; private set;}
	public float MoveSpeed{get;set;}
	public Stats stats;
	public Colour colour;
	public Movement movement;

	private string col;
	private Vector3 dir;
	private GameObject player_obect;
	private Direction current_target = Direction.None;
	private float remainingDistance = 1.0f;

	public Player() {
		stats = new Stats();
		colour = new Colour();
		stats.setHealing(10);
		stats.setDamage(10);
		stats.setHealth(10);
		MoveSpeed = 10.0f;

		col = colour.setColour();
		Debug.Log ("Colour = " + col);

		IsDead = false;
		player_obect = Object.Instantiate(Resources.Load("Prefabs/PlayerPrefab", typeof(GameObject))) as GameObject;
	}

	public bool listenInput() {
        bool validInput = false;
        if (Input.GetKey("w")) {
            validInput = true;
            current_target = Direction.Up;
        } else if (Input.GetKey("a")) {
            current_target = Direction.Left;
            validInput = true;
        } else if (Input.GetKey("s")) {
            current_target = Direction.Down;
            validInput = true;
        } else if (Input.GetKey("d")) {
            current_target = Direction.Right;
            validInput = true;
        } else {
			//Debug.log("Player None direction");
			current_target = Direction.None;
		}
        return validInput;
	}

	public bool checkIfDead() {
		if (stats.getHealth() <= 0) {
			Debug.Log ("player ded");
			IsDead = true;
			FinishedAnimation = true;
			return true;
		}
		return false;
	}

	public void death_tick(){

	}

	public void animation_tick() {
		if (current_target == Direction.None) {
			//there's no current target
			FinishedAnimation = true;
			return;
		}
		switch (current_target) {
			case Direction.Up:
				player_obect.transform.Translate(0, 0, MoveSpeed * Time.deltaTime, null);
				
				break;
			case Direction.Down:
				player_obect.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Left:
				player_obect.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			case Direction.Right:
				player_obect.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			default:
				Debug.Log ("player animation tick no direction!!");
				break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance <= 0) {
			//We've arrived at our destination, but may have overshot, so lets correct it
			Vector3 temp = player_obect.transform.position;
			player_obect.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));

			//reset values, ready for the next player's turn
			current_target = Direction.None;
			remainingDistance = 1.0f;
		}
	}

	//Player input for movement. Must be in update for keypress to work.
	//"not all code paths return a value" 
	public void direction(){
		if(Input.GetKeyDown("w"))
			dir = Vector3.up;
		else if(Input.GetKeyDown("a"))
			dir = Vector3.left;		
		else if(Input.GetKeyDown("s"))
			dir = Vector3.down;		
		else if(Input.GetKeyDown("d"))
			dir = Vector3.right;


		Debug.Log("Direction = " + dir);
		movement.tileJump(dir);
	}

	//current movement jumps the tile, but returns it because of the Vector3.zero in direction();
	/* No longer using monobehaviour
	void Update(){
		direction();
	}
	*/
}