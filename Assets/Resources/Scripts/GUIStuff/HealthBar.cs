using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (!GuiManager.IsShowHealthBar) {
			return;
		}
		if (GameTools.Player != null) {
			//Health Bar
			GUI.Box(new Rect(0, 60,5 * 100 + 10, 40), "Health" );
			if (GameTools.Player.Health < 0)
			{
				//Checks if health goes less than 0 and dsilpay 0
				GUI.Box(new Rect(0, 80,5 * 100 + 10, 20), "0");
			}
			else
			{
				GUI.Box(new Rect(0, 80,5 * 100 + 10, 20), "" + GameTools.Player.Health);
			}
			//Displays bar that decreases based on health
			GUI.Box(new Rect(0, 80,  5*(float)GameTools.Player.Health/(float)GameTools.Player.Health * 100.0f + 10, 20), "");
		}
	}
}
