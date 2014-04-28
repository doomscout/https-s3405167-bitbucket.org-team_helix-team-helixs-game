using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

	void OnGUI()
    {
        GUI.Box(new Rect(250, 250, 180, 100), "YOU WIN"+"\nClick Left Arrow to Continue");

    }
}
