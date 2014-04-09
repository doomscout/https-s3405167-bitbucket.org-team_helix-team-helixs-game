using UnityEngine;
using System.Collections;

public class Buttons : MonoBehaviour {
	int Hp = 50;
	int Mp = 50;
	string Title = "Character Creator";
	int size;
	void Start(){
		 size = Title.Length;
		Debug.Log(size);
	}
	void OnGUI () {

		// Make a background box
		GUI.Box(new Rect(10,10,size * 10,200), "Character Creator" +
			"\nHealth" + "\n\n\nMana");
		//Displays Hit Points
		GUI.Box(new Rect(30,45,(size * 10 - 40),20), Hp.ToString());
		// Click button to decrease Health
		if(GUI.Button(new Rect(10,45,20,20), "<-")) {
			if (Hp != 10)
			{
			Hp -= 10;
			}
	    }
		// Button to increase Health
		if(GUI.Button(new Rect((size * 10 - 10),45,20,20), "->")) {
			Hp += 10;
		}
		GUI.Box(new Rect(30,90,(size * 10 - 40),20), Mp.ToString());
		// Click button to decrease Health

		if(GUI.Button(new Rect(10,90,20,20), "<-")) {
			if (Mp != 10)
			{
				Mp -= 10;
			}
		}
		// Button to increase Health
		if(GUI.Button(new Rect((size * 10 - 10),90,20,20), "->")) {
			Mp += 10;
		}

        if (GUI.Button(new Rect(10, 135,120, 20), "Create Character"))
        {
            Application.LoadLevel("KeYiLevelTest");
        }
	}
}
