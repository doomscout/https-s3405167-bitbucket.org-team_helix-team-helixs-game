using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

    void OnGUI(){
        if ( GUI.Button(new Rect(250,45,200,20), "Start Game")){
            Application.LoadLevel("characterCreator");
        }
        if ( GUI.Button(new Rect(250,65,200,20), "Load Game")){
            
        }
    }
}
