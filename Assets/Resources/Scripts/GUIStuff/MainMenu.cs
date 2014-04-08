using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public bool exit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void OnGUI(){
       if ( GUI.Button(new Rect(10,45,20,20), "Hello")){
            Debug.Log("ButtonPressed");
            exit = true;
            gameObject.SetActive(false);
            exit = true;
                   }
         }
}
