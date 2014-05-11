using UnityEngine;
using System.Collections;

public enum StatusEffects {Poison, Freeze, ReducedDamage, Beserk}

public abstract class Status {
	public int TickCount {get; protected set;}
	public float Power {get; protected set;}
	public StatusEffects StatusEffect {get; protected set;}

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
		Status newStatus = (Status)obj;
		return this.StatusEffect == newStatus.StatusEffect;
	}
	
	public override int GetHashCode () {
		return StatusEffect.ToString().GetHashCode() << 16 | ((int)Power);
	}

}