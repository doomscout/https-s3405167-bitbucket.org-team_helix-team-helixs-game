using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
    public bool paused;
    public bool resume;
    void Start(){
        paused = true;
        resume = false;
    }
void OnGUI(){
        GUI.Box(new Rect(Screen.width/2,Screen.height/2,120,120), "Pause");

        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2 + 30, 120, 30), "Resume")){
           
            resume = true;
            Debug.Log(resume);
            Destroy(gameObject);
        }

        if (GUI.Button(new Rect(Screen.width/2, Screen.height/2 + 60, 120, 30), "Quit")){
            Application.LoadLevel("MenuScene");
        }
    }
}
