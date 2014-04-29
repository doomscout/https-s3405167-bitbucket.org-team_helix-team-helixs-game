using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {
	public Stats stats;
	public Colour PlayerColour;
	public ItemManager deckManager;

	public bool FinishedAnimation{get;set;}
	public bool IsDead{get; private set;}
	public float MoveSpeed{get;set;}

    public int Map_position_x{ get; private set;}
    public int Map_position_y{ get; private set;}

	public GameObject player_object {get; private set;}

	private Direction current_target = Direction.None;
	private float remainingDistance = 1.0f;
	private Spell spell;
	private SpellIndicator spellIndicator;
	private List<float> list_of_damage_taken;
	private List<Colour> list_of_colour_taken;
	private bool castedSpell = false;

	public Player() {
		MoveSpeed = 10.0f;
		stats = new Stats();
		PlayerColour = ColourManager.getRandomColour();
		deckManager = new ItemManager();
		spell = deckManager.peekTopSpell();
		Debug.Log ("Player colour: " + PlayerColour);

		IsDead = false;
		player_object = Object.Instantiate(Resources.Load("Prefabs/PlayerPrefab", typeof(GameObject))) as GameObject;
		player_object.renderer.material.color = ColourManager.toColor(PlayerColour);

		spellIndicator = new SpellIndicator(20);
		list_of_damage_taken = new List<float>();
		list_of_colour_taken = new List<Colour>();

		GameTools.Player = this;
	}

	public void loadIntoGame() {
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
	}

	public void cleanUp() {
		spellIndicator.cleanUp();
		GameTools.Player = null;
		Object.Destroy(player_object);
	}

	public bool listenInput() {
        bool validInput = false;
		castedSpell = false;
		//Keyboard
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
        } else if (Input.GetKey("space")){	//skips turn
			current_target = Direction.None;
			validInput = true;
		}  else  if (Input.GetKeyDown("1")){
			spellIndicator.toggleIndicator();
			current_target = Direction.None;
		} else {
			current_target = Direction.None;
		}

		//Check to see if player is going somewhere invalid
		switch (current_target) {
			case Direction.Down:
				if (GameTools.Map.isOutOfBounds(Map_position_x, Map_position_y-1)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y-1] != null) {
						current_target = Direction.None;
						validInput = false;
					}
				}
				break;
			case Direction.Up:
				if (GameTools.Map.isOutOfBounds(Map_position_x, Map_position_y+1)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y+1] != null) {
						current_target = Direction.None;
						validInput = false;
					}
				}
				break;
			case Direction.Left:
				if (GameTools.Map.isOutOfBounds(Map_position_x-1, Map_position_y)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x-1, Map_position_y] != null) {
						current_target = Direction.None;
						validInput = false;
					}
				}
				break;
			case Direction.Right:
				if (GameTools.Map.isOutOfBounds(Map_position_x+1, Map_position_y)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x+1, Map_position_y] != null) {
						current_target = Direction.None;
						validInput = false;
					}
				}
				break;
			case Direction.None:
			break;
		}

		//Mouse
		if (Input.GetMouseButtonDown(0)) {
			if (spellIndicator.IsShowingIndicator) {
				validInput = castSpell();
			}
		}
        return validInput;
	}

	private bool castSpell() {
		spell.cast(	new int[2] {Map_position_x, Map_position_y},
					new int[2] {GameTools.Mouse.Pos_x, GameTools.Mouse.Pos_z},
					stats.Damage);
		castedSpell = true;
		deckManager.popTopSpell();
		spell = deckManager.peekTopSpell();
		return true;
	}

	public void showIndicatorAnimation() {
		if (!castedSpell) {
			spellIndicator.showNoCastAnimation();
		}
		spellIndicator.showCastAnimation();
	}

	public void showDamageTakenAnimation() {
		for (int i = 0; i < list_of_damage_taken.Count; i++) {
			GameObject o = Object.Instantiate(Resources.Load("Prefabs/DamagePopupPrefab", typeof(GameObject))) as GameObject;
			DamagePopup script = o.GetComponent<DamagePopup>();
			Color c = Color.white;
			if (ColourManager.getStrength(list_of_colour_taken[i]) == PlayerColour) {
				c = Color.magenta;
			} else if (ColourManager.getWeakness(list_of_colour_taken[i]) == PlayerColour) {
				c = Color.grey;
			}
			script.setText(list_of_damage_taken[i] + "");
			script.setColor(c);
			o.transform.position = new Vector3(player_object.transform.position.x, 0, player_object.transform.position.z + 1.0f + i/2.0f);
		}
		list_of_damage_taken = new List<float>();
		list_of_colour_taken = new List<Colour>();
	}

	public void showIndicator() {
		spellIndicator.setSpellIndicator(	new int[2] {Map_position_x, Map_position_y},
											new int[2] {GameTools.Mouse.Pos_x, GameTools.Mouse.Pos_z},
											spell);
	}

	public void getHitByMagic(Spell taken_spell, float power) {
		float modifier = 1.0f;
		if (ColourManager.getWeakness(taken_spell.SpellColour) == PlayerColour) {
			//The spell is weak against our colour
			modifier = ColourManager.WeaknessModifier;
		} else if (ColourManager.getStrength(taken_spell.SpellColour) == PlayerColour) {
			//The spell is strong against us
			modifier = ColourManager.StrengthModifier;
		}
		float damage = taken_spell.Power * modifier * power/10;
		stats.Health -= damage;
		list_of_damage_taken.Add(damage);
		list_of_colour_taken.Add (taken_spell.SpellColour);
	}

	public bool checkIfDead() {
		if (stats.Health <= 0) {
			Debug.Log ("player ded");
			IsDead = true;
			FinishedAnimation = true;
			return true;
		}
		return false;
	}

	public void death_tick(){
		return;
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
	/*
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
	*/
}