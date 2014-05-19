using UnityEngine;
using System.Collections;

public abstract class Effect {
	public int TickCount {get; protected set;}
	public float Power {get; set;}
	
	public Effect(int tickCount, float power) {
		TickCount = tickCount;
		Power = power;
	}
	
	public bool TickDown() {
		TickCount--;
		if (TickCount <= 0) {
			return false;
		}
		return true;
	}

	public abstract string EffectName();
}