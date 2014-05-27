using UnityEngine;
using System.Collections;

public class StatGUI : MonoBehaviour {
    public  GUISkin skin01;
	// Use this for initialization
void OnGUI()
    {
        if (!GuiManager.IsShowStat)
        {
            return;
        }
        GUI.skin = skin01;
        GUI.Box(new Rect(Screen.width * 0, Screen.height * 0.6f, 100f, 100f), "Stats\n" +
                 "Kills: " + GameTools.Player.KillCount + 
                "\nMoney Gained: " + GameTools.Player.MoneyGained);
       
        GUI.Box(new Rect(Screen.width * 0.5f, 0, 100f, 50f), "Turns Left: " + GameTools.GI.NumberOfTurnsUntilWin);
    }
}
