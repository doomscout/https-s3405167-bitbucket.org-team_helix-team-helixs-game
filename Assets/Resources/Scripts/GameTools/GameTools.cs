using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//You can access the gametools from anywhere in the game
//e.g. List<Unit> GameTools.All_Units or TileMap GameTools.Map


public static class GameTools {

	public static List<Unit> All_Units{get;set;}
	public static List<Unit> Dead_Units{get;set;}
	public static TileMap Map{get;set;}
	public static GameManager GM{get;set;}
	public static GameInstance GI{get;set;}
	public static Player Player{get;set;}
	public static TileMouseOver Mouse {get;set;}
	public static CameraInitMove GameCamera {get;set;}
	public static Shop Shop{get;set;}
	public static bool HasRotatedShape = false;

}
