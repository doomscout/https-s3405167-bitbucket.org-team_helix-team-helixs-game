using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDistanceComparer : Comparer<Unit>{

	public override int Compare (Unit x, Unit y) {
		return (int)((GraphSearch	.fromPosition(x.Map_position_x, x.Map_position_y)
								.manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y)
			   		 -GraphSearch	.fromPosition(y.Map_position_x, y.Map_position_y)
		              .manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y)) * 100.0f);
	}
}