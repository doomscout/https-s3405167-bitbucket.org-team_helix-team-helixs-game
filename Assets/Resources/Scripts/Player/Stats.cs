using UnityEngine;
using System.Collections;

public class Stats {
	public float Health {get;set;}
	public float Damage {get;set;}
	public float Healing {get;set;}

	public Stats() {
		Health = 	1000.0f;//((Random.Range(10, 50)) + (Random.Range(50, 100)/3));
		Damage = 	((Random.Range(10, 50)/2) + (Random.Range(50, 100)/10));
		Healing =	((Random.Range(10, 50)/2) + (Random.Range(50, 100)/10));
	}
}