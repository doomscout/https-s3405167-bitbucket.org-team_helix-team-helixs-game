using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private float TimeToShoot;

	public Vector3 initPos;
	public Vector3 destPos;

	float shootCounter = 0.0f;

	bool alertedChild = false;
	bool hasShownDamage = false;
	bool hasGivenTime = false;
	Vector3 randomRotation;

	Spell s;

	// Use this for initialization
	void Start () {
		randomRotation = new Vector3(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));
	}

	public void init(Spell s, Vector3 initPos, Vector3 destPos) {
		this.s = s;
		this.initPos = initPos;
		this.destPos = destPos;
		foreach (Transform t in transform) {
			ProjectileCube pc = t.GetComponent<ProjectileCube>();
			pc.changeColour(ColourManager.toColor(s.SpellColour));
		}
	}

	public void shootIn(float time) {
		TimeToShoot = time;
		hasGivenTime = true;
	}

	public void showDamage() {
		if (!hasShownDamage) {
			hasShownDamage = true;
			s.cast();
			ProjectileManager.getInstance().signalCompletion(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (shootCounter < 1.0f) {
			initPos.y += 3 * Time.deltaTime;
			transform.position = initPos;
			shootCounter += Time.deltaTime;
			transform.Rotate(randomRotation);
		} else if (hasGivenTime) {
			if (TimeToShoot < 0) {
				if (!alertedChild) {
					foreach (ProjectileCube script in transform.GetComponentsInChildren<ProjectileCube>()) {
						script.alert(destPos);
					}
					alertedChild = true;
					Object.Destroy(gameObject, 5.0f);
				}
			} else {
				TimeToShoot -= Time.deltaTime;
				transform.Rotate(randomRotation);
			}
		}
	}
}
