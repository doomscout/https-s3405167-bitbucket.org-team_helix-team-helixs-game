using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	public bool Done {get; private set;}
	public bool Triggered {get; set;}
	public Color colour {get;set;}
	private bool Updated = false;

	// Use this for initialization
	void Start () {
		init ();
	}

	public void init() {
		rigidbody.useGravity = false;
		renderer.material = new Material(Shader.Find ("Transparent/Diffuse"));
		Color c = colour;
		c.a = 0.7f;
		renderer.material.color = c;
		rigidbody.detectCollisions = false;
		Done = false;
		Triggered = false;
	}

	public void changeColour(Color ca) {
		ca.a = 0.7f;
		renderer.material.color = ca;
		colour = ca;
	}
	
	// Update is called once per frame
	void Update () {
		if (!Updated) {
			renderer.material.color = colour;
			Updated = true;
		}
		if (Triggered && !Done) {
			rigidbody.detectCollisions = true;
			rigidbody.useGravity = true;
			Vector3 dir = new Vector3(	((float)Random.Range(-100, 100) )/ 100.0f,
			                          ((float)Random.Range(-100, 100) )/ 100.0f,
			                          ((float)Random.Range(-100, 100) )/ 100.0f);
			
			rigidbody.AddForce(dir * 50);
			rigidbody.AddForce(Vector3.up * 80);
			StartCoroutine("Fade");
			Done = true;
			GameObject.Destroy(gameObject, 4.0f);
		}
	}

	IEnumerator Fade() {
		for (float f = 0.7f; f >= 0; f -= 0.05f) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			yield return new WaitForSeconds(0.1f);
		}
	}

}
