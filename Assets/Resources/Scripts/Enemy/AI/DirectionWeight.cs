using UnityEngine;
using System.Collections;

public class DirectionWeight {

	public Direction d;
	public int weight;
	public int x;
	public int y;
	public int goalX;
	public int goalY;

	public DirectionWeight(Direction da, int weighta, int xa, int ya, int goalXa, int goalYa) {
		d = da;
		weight = weighta;
		x = xa;
		y = ya;
		goalX = goalXa;
		goalY = goalYa;
	}
}
