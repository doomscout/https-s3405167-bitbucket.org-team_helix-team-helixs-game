using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {
   // public GUISkin skin01;
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
			//GUI.Box(new Rect(0, 60,5 * 100 + 10, 40), "Health" );
			
			//Displays bar that decreases based on health
            GUI.color = Color.green;
			GUI.Button(new Rect(0, 80,  5*(float)GameTools.Player.Health/(float)GameTools.Player.Max_Health * 100.0f + 10, 20), "");
            if (GameTools.Player.Health < 0)
            {
                //Checks if health goes less than 0 and dsilpay 0
                GUI.Box(new Rect(0, 80,5 * 100 + 10, 20), "0");
            }
            else
            {  
                GUI.Box(new Rect(0, 80,5 * 100 + 10, 20), "Player Health: " + GameTools.Player.Health);
            }
		}
        GUI.color = Color.gray;
		if (GameTools.Base != null) {
			//Health Bar
			
			//Displays bar that decreases based on health
            GUI.color = Color.blue;
			GUI.Button(new Rect(0, 100,  5*(float)GameTools.Base.Health/(float)GameTools.Base.Max_Health * 100.0f + 10, 20), "");
            if (GameTools.Base.Health < 0)
            {
                //Checks if health goes less than 0 and dsilpay 0
                GUI.Box(new Rect(0, 100,5 * 100 + 10, 20), "0");
            }
            else
            {  GUI.color = Color.white;
                GUI.Box(new Rect(0, 100,5 * 100 + 10, 20), "Base Health: " + GameTools.Base.Health);
            }
		}
	}
}
