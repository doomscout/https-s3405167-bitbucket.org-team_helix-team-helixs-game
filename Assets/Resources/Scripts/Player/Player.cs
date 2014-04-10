using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Stats stats;
	public Colour colour;
	public Movement movement;

	private string col;
	private Vector3 dir;

	void Start(){
		stats.setHealing(10);
		stats.setDamage(10);
		stats.setHealth(10);

		col = colour.setColour();
		Debug.Log ("Colour = " + col);
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
	void Update(){
		direction();
	}
}