using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity {
	public ItemManager deckManager;

	private bool castedSpell = false;
	private SpellIndicator spellIndicator;
	private CastRangeIndicator PlayerCastIndicator;

	public Player() : base(){
		spellIndicator = new SpellIndicator(40);
		PlayerCastIndicator = new CastRangeIndicator();
		spellIndicator.link(PlayerCastIndicator);

		GameTools.Player = this;
	}

	protected override void InitStats() {
		base.InitStats();
		Max_Health = 200.0f;
		Health = Max_Health;
	}

	protected override void InitMapPosition() {
		if (GameTools.Map == null) {
			return;
		}
		for (int i = 0; i < GameTools.Map.size_z; i++) {
			for (int j = 0; j < GameTools.Map.size_x; j++) {
				if (TileTools.IsLand(GameTools.Map.TileMapData[j, i])) {
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
		base.InitMagic();
		deckManager = new ItemManager();
		MainSpell = deckManager.peekTopSpell();
	}

	protected override void InitCleanable () {
		CleanTools.GetInstance().SubscribeCleanable(this, true);
	}

	public override float GetHitByMagic(Spell taken_spell) {
		float dmg = base.GetHitByMagic(taken_spell);
		base.ShowText(dmg + "", ColourManager.toColor(taken_spell.SpellColour), 0);
		return dmg;
	}

	//Only the shop should call this
	public bool BuySpell(Spell s) {
		if (Money - s.SpellRating < 0) {
			Debug.Log ("Can't buy. Current Money: " + Money + ", Price: " + s.SpellRating);
			return false;
		}
		Money -= s.SpellRating;
		deckManager.inv.Add(s);
		return true;
	}
	//Only the shop should call this
	public bool SellSpell(Spell s) {
		if (deckManager.deck.Contains(s)) {
			Debug.Log ("Unequip spell before selling");
			return false;
		}
		if (!deckManager.inv.Contains(s)) {
			Debug.Log ("Player does not have that spell to sell");
			return false;
		}
		Money += s.SpellRating;
		deckManager.inv.Remove(s);
		return true;
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
				if (MapTools.IsOutOfBounds(Map_position_x, Map_position_y-1)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y-1] != null ||
				    	(!TileTools.IsLand(GameTools.Map.TileMapData[Map_position_x, Map_position_y-1]))) {
						current_target = Direction.None;
						validInput = false;
					}
				}
				break;
			case Direction.Up:
				if (MapTools.IsOutOfBounds(Map_position_x, Map_position_y+1)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y+1] != null ||
					    (!TileTools.IsLand(GameTools.Map.TileMapData[Map_position_x, Map_position_y+1]))) {
							current_target = Direction.None;
							validInput = false;
						}
				}
				break;
			case Direction.Left:
				if (MapTools.IsOutOfBounds(Map_position_x-1, Map_position_y)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x-1, Map_position_y] != null ||
					    (!TileTools.IsLand(GameTools.Map.TileMapData[Map_position_x-1, Map_position_y]))) {
							current_target = Direction.None;
							validInput = false;
						}
				}
				break;
			case Direction.Right:
				if (MapTools.IsOutOfBounds(Map_position_x+1, Map_position_y)) {
					current_target = Direction.None;
					validInput = false;
				} else {
					if (GameTools.Map.map_unit_occupy[Map_position_x+1, Map_position_y] != null ||
					    (!TileTools.IsLand(GameTools.Map.TileMapData[Map_position_x+1, Map_position_y]))) {
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

	public new bool IsDead() {
		bool isDead = base.IsDead();
		if (isDead) {
			Debug.Log ("Player is dead");
		}
		return isDead;
	}

	public override bool animation_tick() {
		if (!base.animation_tick()) {
			FinishedAnimation = true;
			return true;
		}
		if (current_target == Direction.None) {
			//there's no current target
			FinishedAnimation = true;
			return true;
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
		return false;
	}
}