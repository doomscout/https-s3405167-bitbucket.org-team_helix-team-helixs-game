using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectionWeightComparer : Comparer<DirectionWeight>{
	
	public override int Compare (DirectionWeight x, DirectionWeight y) {
		int val = 0;
		float d1 = (GraphSearch.fromPosition(x.x, x.y).euclidianDistanceFromTarget(x.goalX, x.goalY));
		float d2 = (GraphSearch.fromPosition(y.x, y.y).euclidianDistanceFromTarget(y.goalX, y.goalY));
		if (d1 < d2) {
			val = -1;
		} else if (d1 > d2) {
			val = 1;
		} else {
			val = 0;
		}

		return (x.weight - y.weight) * 10 + val;
	}
}
