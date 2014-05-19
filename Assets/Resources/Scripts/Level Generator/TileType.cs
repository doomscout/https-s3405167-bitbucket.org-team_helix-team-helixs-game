using UnityEngine;
using System.Collections;

public enum TileType { None, 	LightBlue,		MediumBlue, 	DarkBlue,
								LightGreen,		MediumGreen, 	DarkGreen,
								LightYellow,	MediumYellow,	DarkYellow};


public static class TileTools {

	private static TileType[] HeightMapping = new TileType[6] {	TileType.DarkBlue,
																TileType.MediumBlue,
																TileType.MediumYellow,
																TileType.LightGreen,
																TileType.MediumGreen,
																TileType.DarkGreen};

	public static TileType OuterLandTile = TileType.LightYellow;
	public static TileType InnerLandTile = TileType.DarkGreen;
	public static TileType PoolTile = TileType.MediumBlue;
	public static TileType OceanTile = TileType.DarkBlue;

	public static bool IsLand(TileType t) {
		int i = 0;
		bool found = false;
		for (i = 0; i < HeightMapping.Length; i++) {
			if (t == HeightMapping[i]) {
				found = true;
				break;
			}
		}
		return found && i > 1;
	}

	public static bool IsLand(int t) {
		return t > 1;
	}

	public static bool IsNotLand(int t) {
		return !IsLand(t);
	}

	public static bool IsHigherByMoreThanOne(TileType higher, TileType lower) {
		int lowerIndex = 0;
		int higherIndex = 0;
		for (int i = 0; i < HeightMapping.Length; i++) {
			if (HeightMapping[i] == lower) {
				lowerIndex = i;
			}
			if (HeightMapping[i] == higher) {
				higherIndex = i;
			}
		}
		if (lowerIndex == 0) {
			lowerIndex = 1;
		}
		return higherIndex - lowerIndex > 1;
	}

	public static TileType HeightMappingIncreaseTile(TileType value) {
		int val = 0;
		int i = 0;
		for (i = 0; i < HeightMapping.Length; i++) {
			if (value == HeightMapping[i]) {
				val = i;
				break;
			}
		}
		return HeightMapping[IndexClipping(i)];
	}
	
	private static int IndexClipping(int value) {
		if (value < 1) {
			value = 1;
		}
		if (value >= HeightMapping.Length) {
			value = HeightMapping.Length - 1;
		}
		return value + 1;
	}
}
