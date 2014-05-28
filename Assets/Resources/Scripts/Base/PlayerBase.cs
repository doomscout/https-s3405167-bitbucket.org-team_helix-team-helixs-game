using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBase : Entity {

	private int width;
	private int height;
	public bool HasPlacedBase;

	public int MaxRechargeTime {get;set;}
	public int CastRange{get;set;}
	private int RechargeTime;
	private List<Spell> BufferedSpells;

	public PlayerBase() : base() {
		GameTools.Base = this;
	}

	protected override void PreInit () {
		BufferedSpells = new List<Spell>();
		width = 3;
		height = 3;
		MaxRechargeTime = 7;
		CastRange = 10;
		HasPlacedBase = false;
		name = "Base";
	}

	protected override void InitCleanable() {
		CleanTools.GetInstance().SubscribeCleanable(this, true);
	}

	protected override void InitMapPosition() {
		if (GameTools.Map == null) {
			return;
		}
	}
	
	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/Base", typeof(GameObject))) as GameObject;
		}

	}

	protected override void InitStats() {
		Max_Health = 100.0f;
		Health = Max_Health;
	}

	protected override void InitMagic () {
		base.InitMagic ();
		MainSpell.Shape.CastRange = CastRange;
		MainSpell.CastRange = CastRange;
	}

	public void PlaceBase(int x, int y) {
		HasPlacedBase = true;
		Map_position_x = x;
		Map_position_y = y;
		Debug.Log ("Place base at " + x + ", " + y + " Recharge time " + MaxRechargeTime);
		game_object.transform.position = new Vector3(Map_position_x, 0.1f, Map_position_y);
	}

	public bool IsPlacedOnLand(int x, int y) {
		if (MapTools.IsOutOfBounds(x,y)) {
			return false;
		}
		for (int k = -1; k < 2; k++) {
			for (int l = -1; l < 2; l++) {
				if (MapTools.IsOutOfBounds(x+k, y+l)) {
					return false;
				}
				if (!TileTools.IsLand (GameTools.Map.TileMapData[x+k, y+l])) {
					return false;
				} else {
				}
			}
		}

		return true;
	}

	public void LoadIntoGame() {
		InitMapPosition();
		InitGameObject();
		RechargeTime = MaxRechargeTime;
	}

	public override float GetHitByMagic (Spell taken_spell) {
		float dmg = base.GetHitByMagic (taken_spell);
		if (BufferedSpells.Contains(taken_spell)) {
			return 0.0f;
		}
		Health -= dmg;
		base.ShowText("-" + dmg + " base hp", Color.red,2);
		BattleLog.GetInstance().AddMessage("[Turn " + GameTools.GI.NumberOfTurnsUntilWin +"] " + name + " took " + dmg + " damage.");
		BufferedSpells.Add(taken_spell);
		return dmg;
	}

	public override bool IsEntityAbleToMoveHere (Entity e) {
		return true;
	}

	public bool IsMovingOnTurret(int x, int y) {
		return x == Map_position_x && y == Map_position_y;
		//return false;
	}

	public bool RepairBase() {
		if (Health == Max_Health) {
			Debug.Log ("Too full");
			return false;
		}
		Health += 3;
		if (Health > Max_Health) {
			Health = Max_Health;
		}
		return true;
	}

	public bool UpgradeTurret() {
		if (MaxRechargeTime <= 2) {
			return false;
		}
		MaxRechargeTime--;
		return true;
	}

	public void GetSpellFromPlayer(Spell s, Player p) {
		if (!p.deckManager.deck.Contains(s)) {
			Debug.Log ("Player does not have that spell to give");
		}
		p.deckManager.deck.Remove(s);
		MainSpell = s;
		MainSpell.Shape.ChangeIntoMouse();
		MainSpell.Shape.CastRange = CastRange;
		MainSpell.CastRange = CastRange;
	}

	public bool IsWithinBase(int x, int y) {
		return 	x >= Map_position_x-(width/2)	&& y >= Map_position_y-(height/2) &&
				x <= Map_position_x+(width/2) 	&& y <= Map_position_y+(height/2);
	}

	public void logic_tick() {
		//flush buffered spells
		BufferedSpells = new List<Spell>();
		int castRange = MainSpell.CastRange;
		if (RechargeTime >= MaxRechargeTime) {
			for (int i = -castRange; i <= castRange; i++) {
				for (int j = castRange - Mathf.Abs(i); j >= -(castRange - Mathf.Abs(i)); j--) {
					if (!MapTools.IsOutOfBounds (Map_position_x + i, Map_position_y + j) && GameTools.Map.map_unit_occupy[Map_position_x +  i,Map_position_y + j] != null) {
						MainSpell.loadInfo(	new int[2]{ Map_position_x, Map_position_y},
											new int[2] {Map_position_x + i, Map_position_y + j});
						ProjectileManager.getInstance().queueProjectile(MainSpell, 	game_object.transform.position, 
						                                                			GameTools.Map.map_unit_occupy[Map_position_x +  i,Map_position_y + j].game_object.transform.position);
						RechargeTime = 0;
						i = castRange + 1;
						j = castRange + 1;
						break;
					}
				}
			}
		}
		RechargeTime++;
	}
	
	public new void CleanUp() {
		base.CleanUp();
		GameTools.Base = null;
	}

	//Disable any debuff/status code
	public override void Prelogic_tick () { }
	public override void status_tick () { }
	public override void ApplyStatusOnHit (Spell taken_spell) { }

}
