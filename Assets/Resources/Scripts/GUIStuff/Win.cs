using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {
    public GUISkin skin01;
	void OnGUI()
    {
		if (!GuiManager.IsShowWin) {
			return;
		}
		GUI.skin = null;
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
		GUI.skin = skin01;
		GUI.Box(new Rect(Screen.width * 0.4f , Screen.height * 0.40f, Screen.width * 0.2f, Screen.height * 0.13f), 
		        "You survived day " + (GameTools.GM.NumberOfWins) + "!" +
		        "\n\nStats: " + 
		        "\nKill Count: " +  GameTools.Player.KillCount +
		        "\nDamage Dealt: " + GameTools.Player.DamageDealt +
		        "\nMoney Collected: " + GameTools.Player.MoneyGained);
		ShopButton();
		NextButton();

    }

	private void ShopButton() {
		if (GUI.Button(new Rect(Screen.width * 0.4f , Screen.height * 0.56f, Screen.width * 0.1f, Screen.height * 0.05f), "Shop")) {
			Debug.Log ("Shop button pressed\n");
			GuiManager.IsShowWin = false;
			GuiManager.IsShowShop = true;
		}
	}

	private void NextButton() {
		if (GUI.Button(new Rect(Screen.width * 0.5f , Screen.height * 0.56f, Screen.width * 0.1f, Screen.height * 0.05f), "Next")) {
			Debug.Log ("Next button pressed\n");
			GuiManager.IsShowWin = false;
			GameTools.GM.GoNextLevel = true;
		}
	}
}
