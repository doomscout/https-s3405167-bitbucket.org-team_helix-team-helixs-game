using UnityEngine;
using System.Collections;

public class DamagePopup : MonoBehaviour{

	// Use this for initialization
	void Start () {
		StartCoroutine("Fade");
		StartCoroutine("Rise");
		Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void setText(string s) {
		this.GetComponent<TextMesh>().text = s;
	}

	public void setColor(Color c) {
		this.renderer.material.color = c;
	}

	IEnumerator Fade() {
		for (float f = 1.0f; f >= 0; f -= 0.02f) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			yield return new WaitForSeconds(0.1f);
		}
	}

	IEnumerator Rise() {
		for (float f = 0.0f; f < 3.0f; f += 0.02f) {
			Vector3 t  = transform.position;
			t.z += 0.05f;
			transform.position = t;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
