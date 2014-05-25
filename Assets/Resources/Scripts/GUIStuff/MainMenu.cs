using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public GUISkin skin01;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnGUI(){
		if (!GuiManager.IsShowMainMenu) {
			return;
		}
        GUI.skin = skin01;
		if ( GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height /2 ,200,20), "Start")){
            Debug.Log("StartButton Pressed");
			GameTools.GM.GameStart = true;
			GuiManager.IsShowMainMenu = false;
            }
		if ( GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height /2+25 ,200,20), "Exit")){
			Debug.Log("ExitButton Pressed");
			Application.Quit();
			//Destroy(gameObject);
			GuiManager.IsShowMainMenu = false;
         }
	}
}