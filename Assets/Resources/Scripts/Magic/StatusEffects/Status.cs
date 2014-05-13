using UnityEngine;
using System.Collections;

public enum StatusEffects {Poison, Slow, ReducedDefence}

public abstract class Status {
	public int TickCount {get; protected set;}
	public float Power {get; protected set;}
	public StatusEffects StatusEffect {get; protected set;}

	protected Status(int tickCount, float power) {
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

public class PoisionStatus : Status {

	public PoisionStatus(int tickCount, float power) : base(tickCount, power) {
		StatusEffect = StatusEffects.Poison;
	}
}

public class SlowStatus : Status {
	
	public SlowStatus(int tickCount, float power) : base(tickCount, power) {
		StatusEffect = StatusEffects.Slow;
	}
}

public class ReducedDefenceStatus : Status {
	
	public ReducedDefenceStatus(int tickCount, float power) : base(tickCount, power) {
		StatusEffect = StatusEffects.ReducedDefence;
	}
}