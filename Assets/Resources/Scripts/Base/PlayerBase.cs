using UnityEngine;
using System.Collections;

public class PlayerBase : Entity {

	private int width;
	private int height;

	public int MaxRechargeTime {get;set;}
	private int RechargeTime;

	public PlayerBase() : base() {
		GameTools.Base = this;
		width = 3;
		height = 3;
		MaxRechargeTime = 5;
	}

	protected override void InitCleanable() {
		CleanTools.GetInstance().SubscribeCleanable(this, true);
	}

	protected override void InitMapPosition() {
		if (GameTools.Map == null) {
			return;
		}
		for (int i = 0; i < GameTools.Map.size_z; i++) {
			for (int j = 0; j < GameTools.Map.size_x; j++) {
				if (TileTools.IsLand(GameTools.Map.TileMapData[j, i])) {
					bool isPlaceable = true;
					for (int k = -1; k < 2; k++) {
						for (int l = -1; l < 2; l++) {
							if (!TileTools.IsLand (GameTools.Map.TileMapData[j+k, i+l])) {
								isPlaceable = false;
							}
						}
					}
					if (isPlaceable) {
						Map_position_x = j;
						Map_position_y = i;
						break;
					}
				}
			}
		}
	}
	
	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/Base", typeof(GameObject))) as GameObject;
		}
		game_object.transform.position = new Vector3(Map_position_x, 0.1f, Map_position_y);
	}

	public void LoadIntoGame() {
		InitMapPosition();
		InitGameObject();
		RechargeTime = MaxRechargeTime;
	}

	public override float GetHitByMagic (Spell taken_spell) {
		float dmg = base.GetHitByMagic (taken_spell);
		Health -= dmg;
		base.ShowText("-" + dmg + " base hp", Color.red,2);
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
		int castRange = 5;
		if (RechargeTime >= MaxRechargeTime) {
			for (int i = -castRange; i <= castRange; i++) {
				for (int j = castRange - Mathf.Abs(i); j >= -(castRange - Mathf.Abs(i)); j--) {
					if (!MapTools.IsOutOfBounds (Map_position_x + i, Map_position_y + j) && GameTools.Map.map_unit_occupy[Map_position_x +  i,Map_position_y + j] != null) {
						GameTools.Map.map_unit_occupy[Map_position_x + i, Map_position_y + j].GetHitByMagic(MainSpell);
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
