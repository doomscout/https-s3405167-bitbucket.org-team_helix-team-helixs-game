using UnityEngine;
using System.Collections;

public class PlaceBaseTitle : MonoBehaviour {

	public GUIStyle guistyle;

	// Use this for initialization
	void Start () {
	
	}
	
	void OnGUI() {
		
		if (!GuiManager.IsPlaceBaseTitle) {
			return;
		}
		GUI.Box(new Rect(Screen.width * 0.40f,Screen.height * 0.1f, Screen.width * 0.2f,Screen.height * 0.1f), "Place your base", guistyle);
	}
}
