using UnityEngine;
using System.Collections;

public class StatGUI : MonoBehaviour {
    public  GUISkin skin01;
	public GUIStyle guistyle;
	public GUIStyle guistyle2;
	// Use this for initialization
void OnGUI()
    {
        if (!GuiManager.IsShowStat)
        {
            return;
        }
        GUI.skin = skin01;
        GUI.Box(new Rect(Screen.width * 0, Screen.height * 0.6f, 150f, 100f), "Stats\n" +
                "Kills: " + GameTools.Player.KillCount +
		        "\nDamage dealt: " + GameTools.Player.DamageDealt +
                "\nMoney Gained: " + GameTools.Player.MoneyGained);
       
		GUI.Box(new Rect(Screen.width * 0.48f, Screen.height * 0.05f, Screen.width * 0.04f, 50f), "Turns Left: ", guistyle);
		GUI.Box(new Rect(Screen.width * 0.48f, Screen.height * 0.07f, Screen.width * 0.02f, 50f), "" + GameTools.GI.NumberOfTurnsUntilWin, guistyle2);
    }
}
