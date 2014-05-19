using UnityEngine;
using System.Collections;

public class ShopGUI : MonoBehaviour {

	void OnGUI()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.Box(new Rect(Screen.width * 0.5f, 0,Screen.width * 0.5f, Screen.height ), "Shop" );

        //Shop Display
        float j =1;
        for(int i = 0; i < 10; i++)
        {
             
            GUI.Box(new Rect(Screen.width * 0.6f, Screen.height * 0.1f * j, 300f, 50f ), "Spell" + j);
            GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.1f * j, 100f, 50f), "Buy Spell" + j);
            j += 1;
        }

        //Display Player's Deck/Inventory
        j = 1;
        GUI.Box(new Rect(0, 0, Screen.width * 0.4f, Screen.height), "Deck/Inventory");
        for(int k = 0; k < 10; k++)
        {
            GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.1f * j, 300f, 50f ), "Spell" + j);
            GUI.Button(new Rect(Screen.width * 0.3f, Screen.height * 0.1f * j, 100f, 50f), "Sell Spell" + j);
            j += 1;
        }
    }
}
