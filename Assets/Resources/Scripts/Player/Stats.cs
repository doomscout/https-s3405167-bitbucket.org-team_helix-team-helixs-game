using UnityEngine;
using System.Collections;

public class Stats {
	public float Health {get;set;}
	public float Max_Health {get;set;}
	public float Healing {get;set;}

	public Stats() {
		Health = 	200.0f;//((Random.Range(10, 50)) + (Random.Range(50, 100)/3));
		Max_Health = Health;
		Healing =	Random.Range(1, 5);
	}
}