using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarComparer : Comparer<AStarNode>{

	public override int Compare (AStarNode x, AStarNode y) {
		return x.FScore - y.FScore;
	}
}
