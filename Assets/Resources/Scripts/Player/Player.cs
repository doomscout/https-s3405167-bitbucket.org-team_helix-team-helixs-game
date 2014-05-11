using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity {
	public ItemManager deckManager;

	private bool castedSpell = false;
	private SpellIndicator spellIndicator;
	private CastRangeIndicator PlayerCastIndicator;

	public Player() {
		spellIndicator = new SpellIndicator(20);
		PlayerCastIndicator = new CastRangeIndicator();
		spellIndicator.link(PlayerCastIndicator);

		GameTools.Player = this;
	}

	protected override void InitStats() {
		Max_Health = 200.0f;
		Health = Max_Health;
	}

	protected override void InitMapPosition() {
		if (GameTools.Map == null) {
			return;
		}
		for (int i = 0; i < GameTools.Map.size_z; i++) {
			for (int j = 0; j < GameTools.Map.size_x; j++) {
				if (GameTools.Map.store_data[j, i] != Colour.None) {
					Map_position_x = j;
					Map_position_y = i;
					break;
				}
			}
		}
	}

	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/PlayerPrefab", typeof(GameObject))) as GameObject;
		}
		game_object.transform.position = new Vector3(Map_position_x, 0, Map_position_y);
		game_object.renderer.material.color = ColourManager.toColor(MainColour);
	}

	protected override void InitMagic() {
		deckManager = new ItemManager();
		MainSpell = deckManager.peekTopSpell();
	}

	public void loadIntoGame() {
		InitMapPosition();
		InitGameObject();
	}

	public new void CleanUp() {
		base.CleanUp();
		GameTools.Player = null;
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
			//stats.Health += stats.Healing;
			validInput = true;
		}  else  if (Input.GetKeyUp("1")){
			spellIndicator.toggleIndicator();
			PlayerCastIndicator.ToggleUnit(this);
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
					if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y-1] != null ||
				    	GameTools.Map.store_data[Map_position_x, Map_position_y-1] == Colour.None) {
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
					if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y+1] != null ||
					    GameTools.Map.store_data[Map_position_x, Map_position_y+1] == Colour.None) {
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
					if (GameTools.Map.map_unit_occupy[Map_position_x-1, Map_position_y] != null ||
					    GameTools.Map.store_data[Map_position_x-1, Map_position_y] == Colour.None) {
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
					if (GameTools.Map.map_unit_occupy[Map_position_x+1, Map_position_y] != null ||
					    GameTools.Map.store_data[Map_position_x+1, Map_position_y] == Colour.None) {
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
				validInput = CastMainSpell();
			}
		}
        return validInput;
	}

	private new bool CastMainSpell() {
		base.CastMainSpell ();
		MainSpell.cast(		new int[2] {Map_position_x, Map_position_y},
							new int[2] {GameTools.Mouse.Pos_x, GameTools.Mouse.Pos_z});
		castedSpell = true;
		deckManager.popTopSpell();
		MainSpell = deckManager.peekTopSpell();
		return true;
	}

	public void showIndicatorAnimation() {
		if (!castedSpell) {
			spellIndicator.showNoCastAnimation();
		}
		spellIndicator.showCastAnimation();
		PlayerCastIndicator.ResetIndicators();
	}

	public void showIndicator() {
		spellIndicator.setSpellIndicator(	new int[2] {Map_position_x, Map_position_y},
											new int[2] {GameTools.Mouse.Pos_x, GameTools.Mouse.Pos_z},
											MainSpell);
		PlayerCastIndicator.ShowIndicators();

	}

	public bool checkIfDead() {
		if (Health <= 0) {
			Debug.Log ("player ded");
			IsDead = true;
			FinishedAnimation = true;
			return true;
		}
		return false;
	}

	public override void animation_tick() {
		if (current_target == Direction.None) {
			//there's no current target
			FinishedAnimation = true;
			return;
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
				Debug.Log ("player animation tick no direction!!");
				break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance <= 0) {
			//We've arrived at our destination, but may have overshot, so lets correct it
			Vector3 temp = game_object.transform.position;
			game_object.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));
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

			//reset values, ready for the next player's turn
			current_target = Direction.None;
			remainingDistance = 1.0f;
		}
	}
}