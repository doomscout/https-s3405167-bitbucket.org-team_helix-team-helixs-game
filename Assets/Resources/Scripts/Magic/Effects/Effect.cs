using UnityEngine;
using System.Collections;

public enum EffectType {Poison, Slow, ReducedDefence}

public class Effect {
	public int TickCount {get; protected set;}
	public float Power {get; protected set;}
	public EffectType StatusEffect {get; protected set;}

	public Effect(int tickCount, float power, EffectType se) {
		TickCount = tickCount;
		Power = power;
		StatusEffect = se;
	}

	public bool TickDown() {
		TickCount--;
		if (TickCount <= 0) {
			return false;
		}
		return true;
	}
	
	public override bool Equals (object obj) {
		if (obj == null || this.GetType() != obj.GetType()) {
			return false;
		}
		Effect newStatus = (Effect)obj;
		return this.StatusEffect == newStatus.StatusEffect;
	}
	
	public override int GetHashCode () {
		return StatusEffect.ToString().GetHashCode() << 16 | ((int)Power);
	}

}