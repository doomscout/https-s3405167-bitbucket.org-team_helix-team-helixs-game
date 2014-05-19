using UnityEngine;
using System.Collections;

public enum StatusType {Poison, Ensnare, ReducedDefence}

public class StatusEffect : Effect{
	public StatusType Status {get; protected set;}

	public StatusEffect(int tickCount, float power, StatusType se) : base(tickCount, power){
		Status = se;
	}
	
	public override bool Equals (object obj) {
		if (obj == null || this.GetType() != obj.GetType()) {
			return false;
		}
		StatusEffect newStatus = (StatusEffect)obj;
		return this.Status == newStatus.Status && this.Power == newStatus.Power && TickCount == newStatus.TickCount;
	}
	
	public override int GetHashCode () {
		return Status.ToString().GetHashCode() << 16 | ((int)Power);
	}

	public override string EffectName () {
		return Status.ToString();
	}
}