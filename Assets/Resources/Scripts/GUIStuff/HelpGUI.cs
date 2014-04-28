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
                 +"\n Space Bar - Skip Turn"   );
        }

       
            Debug.Log("GUIComplete");
		if (GameTools.Player != null) {
			GUI.Box(new Rect(100, 100, (float)GameTools.Player.stats.Health/(float)GameTools.Player.stats.Max_Health * 100.0f + 10, 20), "" + GameTools.Player.stats.Health);
		}
       
    }
}
