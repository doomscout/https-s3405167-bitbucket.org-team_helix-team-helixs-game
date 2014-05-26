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
		if (!GuiManager.IsShowHelp) {
			return;
		}
        if(GUI.Button(new Rect(0,100,20,20), "?")){
            helpOn = !helpOn;
        }
        if (helpOn == true)
        {
            GUI.Box(new Rect(0, 120, 350, 250), "Help Content" +
                "\nMovement - WASD keys" +
                "\nPress 1 and select Area- to Cast Spell"
                 +"\n Space Bar - Skip Turn" + 
                 "\n\n Objective: \nDefend the Base until the number of turns is zero" 
                    +"\n\nTile Effect:" + "\nRed Tiles: Heals Player"
                    +"\nYellow Tiles: Gain Money"
                    +"\nCircle: Step to gain Money or health"
                    + "\nSquare: Stand kill to gain Money or Health");
        }
       
    }
}
