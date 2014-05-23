using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 180f, 100f), "YOU LOSE" +
            "\nClick Left Arrow to Continue");
        
    }
}
