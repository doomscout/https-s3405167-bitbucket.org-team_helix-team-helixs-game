using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {
	private float health;
	private float damage;
	private float healing;

	public void setHealth(float seed){
		float stat = ((Random.Range(10, 50)) + (Random.Range(50, 100)/3) + seed);
		Debug.Log ("HP = " + stat);
		health = stat;
	}

	public void setDamage(float seed){
		float stat = ((Random.Range(10, 50)/2) + (Random.Range(50, 100)/10) + seed);
		Debug.Log ("DMG = " + stat);
		damage = stat;
	}

	public void setHealing(float seed){
		float stat = ((Random.Range(10, 50)/2) + (Random.Range(50, 100)/10) + seed);
		Debug.Log ("HL = " + stat);
		healing = stat;
	}

	public float getHealth(){
		return health;
	}

	public float getDamage(){
		return damage;
	}

	public float getHealing(){
		return healing;
	}
}