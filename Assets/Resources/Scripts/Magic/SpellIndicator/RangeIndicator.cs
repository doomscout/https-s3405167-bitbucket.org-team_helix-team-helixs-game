using UnityEngine;
using System.Collections;

public class RangeIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		renderer.material = new Material(Shader.Find ("Transparent/Diffuse"));
		Color c = Color.grey;
		c.a = 0.5f;
		renderer.material.color = c;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
