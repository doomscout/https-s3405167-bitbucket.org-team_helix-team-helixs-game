using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	void OnGUI()
    {
		if (!GuiManager.IsShowWin) {
			return;
		}
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
		GUI.Box(new Rect(Screen.width * 0.4f , Screen.height * 0.4f, Screen.width * 0.2f, Screen.height * 0.1f), "YOU WIN"+"\nClick Left Arrow to Continue");

		ShopButton();
		NextButton();

    }

	private void ShopButton() {
		if (GUI.Button(new Rect(Screen.width * 0.4f , Screen.height * 0.55f, Screen.width * 0.1f, Screen.height * 0.05f), "Shop")) {
			Debug.Log ("Shop button pressed\n");
			GuiManager.IsShowWin = false;
			GuiManager.IsShowShop = true;
		}
	}

	private void NextButton() {
		if (GUI.Button(new Rect(Screen.width * 0.5f , Screen.height * 0.55f, Screen.width * 0.1f, Screen.height * 0.05f), "Next")) {
			Debug.Log ("Next button pressed\n");
			GuiManager.IsShowWin = false;
			GameTools.GM.GoNextLevel = true;
		}
	}
}
