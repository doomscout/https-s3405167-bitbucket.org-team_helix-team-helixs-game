using UnityEngine;
using System.Collections;

public class BattleLogGui : MonoBehaviour {

	private Vector2 scrollPos;
	public string textToDisplay;

	// Use this for initialization
	void Start () {
		scrollPos = new Vector2(2, 400);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnGUI(){
		if (!GuiManager.IsShowBattleLog) {
			return;
		}
		//Dark box that is the same dimensions as battle log so that battlelog has dark background.
		GUI.Box(new Rect(0, Screen.height * 0.8f, Screen.width * 0.3f, Screen.height * 0.20f), "");
		string[] log = BattleLog.GetInstance().Log.ToArray();
		scrollPos = GUI.BeginScrollView(new Rect(0, Screen.height * 0.8f, Screen.width * 0.3f, Screen.height * 0.20f), scrollPos, new Rect(0, 0, Screen.width * 0.3f - 25, 400));

		for (int i = 0; i < log.Length; i++) {
			GUILayout.Label(log[i]);
		}

		GUI.EndScrollView();

		
	}
}
