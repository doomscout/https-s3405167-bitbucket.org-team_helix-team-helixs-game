using UnityEngine;
using System.Collections;

public static class MapTools {
	
	public static bool IsOutOfBounds(int x, int y) {
		return 	x < 0 || x >= GameTools.Map.size_x ||
			y < 0 || y >= GameTools.Map.size_z;
	}

}
