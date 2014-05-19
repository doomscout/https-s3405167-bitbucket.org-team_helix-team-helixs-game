using UnityEngine;
using System.Collections;

public class PlayerBase : Entity {
	
	
	protected override void InitMapPosition() {
		//TODO
		Map_position_x = 0;
		Map_position_y = 0;
	}
	protected override void InitGameObject() {
		if (base.game_object == null) {
			base.game_object = Object.Instantiate(Resources.Load("Prefabs/Base", typeof(GameObject))) as GameObject;
		}
		game_object.transform.position = new Vector3(Map_position_x, 0, Map_position_y);
	}

	protected override void InitCleanable() {
		//TODO
	}

	public override bool EntityAbleToMoveHere (Entity e) {
		return e.GetType() == typeof(Player);
	}



	//Disable any debuff/status code
	public override void Prelogic_tick () { }
	public override void status_tick () { }
	public override void ApplyStatusOnHit (Spell taken_spell) { }

}
