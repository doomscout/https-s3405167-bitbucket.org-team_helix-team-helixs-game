using UnityEngine;
using System.Collections;

public class Player {
	public bool FinishedAnimation{get;set;}
	public bool IsDead{get; private set;}
	public float MoveSpeed{get;set;}
	public Stats stats;
	public Colour colour;
	public Movement movement;
    public int Map_position_x{ get; private set;}
    public int Map_position_y{ get; private set;}

	private string col;
	private Vector3 dir;
	private GameObject player_object;
	private Direction current_target = Direction.None;
	private float remainingDistance = 1.0f;


	public Player() {
		MoveSpeed = 10.0f;
		stats = new Stats();
		stats.setHealth(10);
		colour = (Colour)Random.Range(1, 6);
		Debug.Log ("Player colour: " + colour);

		IsDead = false;
		player_object = Object.Instantiate(Resources.Load("Prefabs/PlayerPrefab", typeof(GameObject))) as GameObject;
		for (int i = 0; i < GameTools.Map.size_z; i++) {
			for (int j = 0; j < GameTools.Map.size_x; j++) {
				if (GameTools.Map.store_data[j, i] != Colour.None) {
					Map_position_x = j;
					Map_position_y = i;
					break;
				}
			}
		}
		player_object.transform.position = new Vector3(Map_position_x, 0, Map_position_y);

		GameTools.Player = this;
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
        } else if (Input.GetKey("space")){
			current_target = Direction.None;
			validInput = true;
		} else {
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
				player_object.transform.Translate(0, 0, MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Down:
				player_object.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Left:
				player_object.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			case Direction.Right:
				player_object.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			default:
				Debug.Log ("player animation tick no direction!!");
				break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance <= 0) {
			//We've arrived at our destination, but may have overshot, so lets correct it
			Vector3 temp = player_object.transform.position;
			player_object.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));
            switch (current_target) {
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
            //Debug.Log("Arrived at (" + Map_position_x + ", " + Map_position_y + ")");
            //Debug.Log("Number is" + GameTools.Map[Map_position_y, Map_position_x]);

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
}