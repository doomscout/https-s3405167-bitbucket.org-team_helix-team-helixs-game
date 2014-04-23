using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarComparer : Comparer<AStarNode>{

	//Since the compare is int, we multiply it to increase precision
	public static readonly float precision = 100.0f;

	public override int Compare (AStarNode x, AStarNode y) {
		return (int)(x.getFScore() * precision - y.getFScore() * precision);
	}
}
