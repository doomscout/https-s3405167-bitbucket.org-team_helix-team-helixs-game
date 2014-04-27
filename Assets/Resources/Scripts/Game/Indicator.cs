using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TriggerAnimation() {
		foreach(Transform child in transform) {
			Test script = child.GetComponent<Test>();
			script.Triggered = true;
		}
		GameObject.Destroy(gameObject, 4.0f);
	}
}
