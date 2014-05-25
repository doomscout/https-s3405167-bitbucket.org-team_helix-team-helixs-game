using UnityEngine;
using System.Collections;

public class ProjectileCube : MonoBehaviour {

	float time = 0.0f;
	bool alerted = false;

	Color c;

	// Use this for initialization
	void Start () {
		rigidbody.detectCollisions = false;
		renderer.material = new Material(Shader.Find ("Transparent/Diffuse"));
		renderer.material.color = Color.black;
	}

	public void changeColour(Color ca) {
		ca.a = 1.0f;
		c = ca;
		renderer.material.color = c;
	}

	public void alert(Vector3 destPos) {
		alerted = true;
		float pox = destPos.x - transform.position.x; 
		float poy = destPos.y - transform.position.y + 0.5f;
		float poz = destPos.z - transform.position.z;
		Vector3 v = new Vector3(pox, poy, poz);
		v.Normalize();
		rigidbody.AddForce(v * 2000);
		rigidbody.detectCollisions = true;
		rigidbody.AddTorque(50000, 2000, 4000);
		StartCoroutine("Fade");
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = c;
		if (alerted) {
			time += Time.deltaTime;
			if (time > 0.5) {
				rigidbody.useGravity = true;
			}
		}
	}

	public void OnCollisionEnter(Collision c) {
		if (GameTools.Player == null || GameTools.Player.game_object == null) {
			return;
		}
		if (c.collider == GameTools.Player.game_object.collider) {
			transform.parent.GetComponent<Projectile>().showDamage();
		} else if (c.collider == GameTools.Base.game_object.collider) {
			transform.parent.GetComponent<Projectile>().showDamage();
		} else if (c.collider == c.transform.GetComponent<SphereCollider>()) {
			Debug.Log ("hit sphere collider");
			transform.parent.GetComponent<Projectile>().showDamage();
		}
	}

	IEnumerator Fade() {
		for (float f = 1.0f; f >= 0; f -= 0.05f) {
			c.a = f;
			renderer.material.color = c;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
