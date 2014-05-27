using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerBase : Entity {

	private int width;
	private int height;
	public bool HasPlacedBase;

	public int MaxRechargeTime {get;set;}
	private int RechargeTime;
	private List<Spell> BufferedSpells;

	public PlayerBase() : base() {
		GameTools.Base = this;
		BufferedSpells = new List<Spell>();
		width = 3;
		height = 3;
		MaxRechargeTime = 7;
		HasPlacedBase = false;
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
		MainSpell.Shape.CastRange = 15;
	}

	public void PlaceBase(int x, int y) {
		HasPlacedBase = true;
		Map_position_x = x;
		Map_position_y = y;
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
		BattleLog.GetInstance().AddMessage("[Turn " + GameTools.GI.NumberOfTurnsUntilWin +"] Base took " + dmg + " damage.");
		BufferedSpells.Add(taken_spell);
		return dmg;
	}

	public override bool IsEntityAbleToMoveHere (Entity e) {
		return e.GetType() == typeof(Player);
	}

	public bool IsWithinBase(int x, int y) {
		return 	x >= Map_position_x-(width/2)	&& y >= Map_position_y-(height/2) &&
				x <= Map_position_x+(width/2) 	&& y <= Map_position_y+(height/2);
	}

	public void logic_tick() {
		//flush buffered spells
		BufferedSpells = new List<Spell>();
		int castRange = 15;
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
