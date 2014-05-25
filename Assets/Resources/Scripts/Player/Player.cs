using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity {
	public ItemManager deckManager;
	private Animator playerAnimation;
	private bool castedSpell = false;
	private SpellIndicator spellIndicator;
	private CastRangeIndicator PlayerCastIndicator;

	public Player() : base(){
		spellIndicator = new SpellIndicator(40);
		PlayerCastIndicator = new CastRangeIndicator();
		spellIndicator.link(PlayerCastIndicator);

		GameTools.Player = this;
		SpellGenerator.GetInstance().PrintAllSpells();
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
		Map_position_x = GameTools.Base.Map_position_x;
		Map_position_y = GameTools.Base.Map_position_y;
	}

	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/slime", typeof(GameObject))) as GameObject;
		}
		playerAnimation = game_object.GetComponent<Animator> ();
		game_object.transform.position = new Vector3(Map_position_x, 0.1f, Map_position_y);
		//game_object.renderer.material.color = ColourManager.toColor(MainColour);
	}

	protected override void InitMagic() {
		base.InitMagic();
		deckManager = new ItemManager();
		MainSpell = deckManager.peekTopSpell();
	}

	protected override void InitCleanable () {
		CleanTools.GetInstance().SubscribeCleanable(this, true);
	}
	
	public void LoadIntoGame() {
		InitMapPosition();
		InitGameObject();
	}
	
	public new void CleanUp() {
		base.CleanUp();
		GameTools.Player = null;
	}

	public void ReloadSpell() {
		MainSpell = deckManager.peekTopSpell();
	}

	public override float GetHitByMagic(Spell taken_spell) {
		float dmg = base.GetHitByMagic(taken_spell);
		if (GameTools.Base.IsWithinBase(Map_position_x, Map_position_y)) {
			GameTools.Base.GetHitByMagic(taken_spell);
		} else {
			Health -= dmg;
			base.ShowText("-" + dmg + "Player HP", Color.red, 0);
			BattleLog.GetInstance().AddMessage("[Turn " + GameTools.GI.NumberOfTurnsUntilWin +"] Player took " + dmg + " damage.");
		}
		return dmg;
	}

	//Only the shop should call this
	public bool BuySpell(Spell s) {
		if (Money - s.SpellRating < 0) {
			Debug.Log ("Can't buy. Current Money: " + Money + ", Price: " + s.SpellRating);
			return false;
		}
		Money -= s.SpellRating;
		deckManager.deck.Add(s);
		return true;
	}
	//Only the shop should call this
	public bool SellSpell(Spell s) {
		if (deckManager.deck.Contains(s)) {
			deckManager.deck.Remove(s);
			deckManager.inv.Add(s);
		}
		if (!deckManager.inv.Contains(s)) {
			Debug.Log ("Player does not have that spell to sell");
			return false;
		}
		Money += s.SpellRating;
		deckManager.inv.Remove(s);
		return true;
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
		}  else  if (Input.GetKeyUp("1")){
			spellIndicator.toggleIndicator();
			PlayerCastIndicator.ToggleUnit(this);
			current_target = Direction.None;
		} else {
			current_target = Direction.None;
		}

		//Check to see if player is going somewhere invalid
		int newX = 0, newY = 0;
		switch (current_target) {
			case Direction.Down:
			newX = Map_position_x;
			newY = Map_position_y-1;
			if (MapTools.IsOutOfBounds(newX, newY)) {
				current_target = Direction.None;
				validInput = false;
			} else {
				if ((GameTools.Map.map_unit_occupy[newX, newY] != null &&
				     !GameTools.Map.map_unit_occupy[newX, newY].IsEntityAbleToMoveHere(this))||
				    (!TileTools.IsLand(GameTools.Map.TileMapData[newX, newY]))) {
					current_target = Direction.None;
					validInput = false;
				}
			}
			break;
			case Direction.Up:
			newX = Map_position_x;
			newY = Map_position_y+1;
			if (MapTools.IsOutOfBounds(newX, newY)) {
				current_target = Direction.None;
				validInput = false;
			} else {
				if ((GameTools.Map.map_unit_occupy[newX, newY] != null &&
				     !GameTools.Map.map_unit_occupy[newX, newY].IsEntityAbleToMoveHere(this))||
				    (!TileTools.IsLand(GameTools.Map.TileMapData[newX, newY]))) {
					current_target = Direction.None;
					validInput = false;
				}
			}
			break;
			case Direction.Left:
			newX = Map_position_x-1;
			newY = Map_position_y;
				if (MapTools.IsOutOfBounds(newX, newY)) {
					current_target = Direction.None;
					validInput = false;
				} else {
				if ((GameTools.Map.map_unit_occupy[newX, newY] != null &&
				     !GameTools.Map.map_unit_occupy[newX, newY].IsEntityAbleToMoveHere(this))||
				    (!TileTools.IsLand(GameTools.Map.TileMapData[newX, newY]))) {
							current_target = Direction.None;
							validInput = false;
						}
				}
				break;
			case Direction.Right:
			newX = Map_position_x+1;
			newY = Map_position_y;
			if (MapTools.IsOutOfBounds(newX, newY)) {
				current_target = Direction.None;
				validInput = false;
			} else {
				if ((GameTools.Map.map_unit_occupy[newX, newY] != null &&
				     !GameTools.Map.map_unit_occupy[newX, newY].IsEntityAbleToMoveHere(this))||
				    (!TileTools.IsLand(GameTools.Map.TileMapData[newX, newY]))) {
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
		playerAnimation.SetBool ("Cast", true);
		castedSpell = true;
		deckManager.popTopSpell();
		ReloadSpell();
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
			playerAnimation.SetBool ("Death", true);
		}

		return isDead;
	}

	public override bool animation_tick() {
		playerAnimation.SetBool ("Cast", false);
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
				playerAnimation.SetInteger("Direction", 0);
				break;
			case Direction.Down:
				game_object.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
				playerAnimation.SetInteger("Direction", 1);
				break;
			case Direction.Left:
				game_object.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
				playerAnimation.SetInteger("Direction", 2);
				break;
			case Direction.Right:
				game_object.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
				playerAnimation.SetInteger("Direction", 3);
				break;
			default:
				Debug.Log ("player animation tick no direction!!");
				break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance <= 0) {
			//We've arrived at our destination, but may have overshot, so lets correct it
			playerAnimation.SetInteger("Direction", 4);
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