using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    void Start(){

    }

	void OnGUI() {

		if (!GuiManager.IsPause) {
			return;
		}
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.Box(new Rect(Screen.width/2,Screen.height/2,120,120), "Pause");

        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2 + 30, 120, 30), "Resume")){
			GameTools.GM.ResumeGame = true;
			GuiManager.IsPause = false;
        }

        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2 + 60, 120, 30), "Quit")){
			GameTools.GM.QuitGame = true;
			GuiManager.IsPause = false;
        }
    }
}
	