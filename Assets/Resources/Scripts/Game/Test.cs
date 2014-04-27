using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public bool Done {get; private set;}
	public bool Triggered {get; set;}
	// Use this for initialization
	void Start () {
		rigidbody.useGravity = false;
		renderer.material.color = Color.cyan;
		rigidbody.detectCollisions = false;
		Done = false;
		Triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Triggered && !Done) {
			rigidbody.detectCollisions = true;
			rigidbody.useGravity = true;
			Vector3 dir = new Vector3(	((float)Random.Range(-100, 100) )/ 100.0f,
			                          ((float)Random.Range(-100, 100) )/ 100.0f,
			                          ((float)Random.Range(-100, 100) )/ 100.0f);
			
			rigidbody.AddForce(dir * 50);
			rigidbody.AddForce(Vector3.up * 80);
			Done = true;
			GameObject.Destroy(gameObject, 3.0f);
			Debug.Log ("asdasdsadasdasd");
		}
	}
}
