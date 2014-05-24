using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GuiManager {

	public static bool IsShowMainMenu = false;
	public static bool IsShowWin = false;
	public static bool IsShowLose = false;
	public static bool IsShowShop = false;
	public static bool IsPause = false;
	public static bool IsShowHelp = false;
	public static bool isShowInventory = false;
	public static bool IsShowHealthBar = false;

	public static void Reset() {
	 	IsShowMainMenu = false;
	 	IsShowWin = false;
	 	IsShowLose = false;
	 	IsShowShop = false;
	 	IsPause = false;
	 	IsShowHelp = false;
	 	isShowInventory = false;
	 	IsShowHealthBar = false;
	}
}
