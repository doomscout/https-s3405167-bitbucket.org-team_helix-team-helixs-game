using UnityEngine;
using System.Collections;

public class Lose : MonoBehaviour {

    void OnGUI()
    {
        GUI.Box(new Rect(250, 250, 180, 100), "YOU LOSE" +
            "\nClick Left Arrow to Continue");
        
    }
}
