using UnityEngine;
using System.Collections;

public class HelpGUI : MonoBehaviour {
    public bool helpOn;

	// Use this for initialization
	void Start () {
        helpOn = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI(){
        if(GUI.Button(new Rect(0,100,20,20), "?")){
            helpOn = !helpOn;
        }
        if (helpOn == true)
        {
            GUI.Box(new Rect(0, 120, 250, 100), "Help Content" +
                "\nMovement - WASD keys" +
                "\nPress 1 and select Area- to Cast Spell"
                 +"\n Space Bar - Skip Turn" + "\n Press 'i' for Inventory"   );
        }

       
            //Debug.Log("GUIComplete");
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
