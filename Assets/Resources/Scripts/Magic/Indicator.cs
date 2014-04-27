using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void changeColour(Color c) {
		foreach(Transform child in transform) {
			Test script = child.GetComponent<Test>();
			script.changeColour(c);
		}
	}

	public void TriggerAnimation() {
		foreach(Transform child in transform) {
			Test script = child.GetComponent<Test>();
			script.Triggered = true;
		}
		GameObject.Destroy(gameObject, 5.0f);
	}
}
