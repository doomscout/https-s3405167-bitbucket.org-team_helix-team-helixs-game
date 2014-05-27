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
		GUI.Box(new Rect(Screen.width * 0.45f,Screen.height * 0.45f, Screen.width * 0.1f,Screen.height * 0.1f), "Pause");

		if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height * 0.45f + 30f, Screen.width * 0.1f, 30f), "Resume")){
			GameTools.GM.ResumeGame = true;
			GuiManager.IsPause = false;
        }

        if (GUI.Button(new Rect(Screen.width * 0.45f, Screen.height* 0.45f + 60f, Screen.width * 0.1f, 30f), "Quit")){
			GameTools.GM.QuitGame = true;
			GuiManager.IsPause = false;
        }

		if (Input.GetKeyUp("escape")) {
			GuiManager.IsPause = false;
			GameTools.GM.ResumeGame = true;
		}
    }
}
	