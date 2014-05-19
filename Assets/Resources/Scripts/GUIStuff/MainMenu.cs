using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public bool exit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnGUI(){
       if ( GUI.Button(new Rect(10,45,200,20), "Start")){
            Debug.Log("ButtonPressed");
            exit = true;
            Destroy(gameObject);
                   }
         }
}
