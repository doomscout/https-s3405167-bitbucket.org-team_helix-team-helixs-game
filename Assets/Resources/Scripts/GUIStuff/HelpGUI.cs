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
        GUI.Box(new Rect(100, 100, GameTools.Player.stats.Health/10, 20), "Health" + GameTools.Player.stats.Health);
       
    }
}
