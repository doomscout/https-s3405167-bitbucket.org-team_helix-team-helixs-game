using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public bool exit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnGUI(){
		if ( GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height /2 ,200,20), "Start")){
            Debug.Log("StartButton Pressed");
            exit = true;
            Destroy(gameObject);
            }
		if ( GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height /2+25 ,200,20), "Exit")){
			Debug.Log("ExitButton Pressed");
			Application.Quit();
			//Destroy(gameObject);
         }
	}
}