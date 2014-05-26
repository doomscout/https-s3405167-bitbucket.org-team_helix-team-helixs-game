using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {
    public GUISkin skin01;
    void OnGUI()
    {
		if (!GuiManager.IsShowLose) {
			return;
		}
        GUI.skin = skin01;
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
		if(GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.45f,Screen.width * 0.1f, Screen.height * 0.1f), "YOU LOSE")) {
			GuiManager.IsShowLose = false;
			GameTools.GM.QuitGame = true;
		}
        
    }
}
