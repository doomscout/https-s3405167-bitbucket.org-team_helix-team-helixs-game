using UnityEngine;
using System.Collections;

public enum TileType { Sea, Grass, Hill, Forest};


public static class TileManager{

	public static readonly float WeaknessModifier = 1.5f;

	public static TileType getRandomTileType(){
		return (TileType)Random.Range (1,System.Enum.GetNames(typeof(TileType)).Length - 1);
	}
	/*
	public static TileType toTileType(TileType t){
		switch(t){
		case TileType.Grass:
			return Colour.green;
		case TileType.Forest:
			return Colour.
		
		}
	
	}
	*/
}
