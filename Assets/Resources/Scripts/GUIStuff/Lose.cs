using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {
    public GUISkin skin01;
    void OnGUI()
    {
		if (!GuiManager.IsShowLose) {
			return;
		}
		GUI.skin = null;
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
		GUI.skin = skin01;
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.45f,Screen.width * 0.1f, Screen.height * 0.1f), "YOU LOSE")) {
			GuiManager.IsShowLose = false;
			GameTools.GM.QuitGame = true;
		}
		GUI.Box(new Rect(Screen.width * 0.45f, Screen.height * 0.55f,Screen.width * 0.1f, Screen.height * 0.15f), 
		        "Stats: " + 
		        "\nKill Count: " +  GameTools.Player.KillCount +
		        "\nDamage Dealt: " + GameTools.Player.DamageDealt +
		        "\nMoney Collected: " + GameTools.Player.MoneyGained);
    }
}
