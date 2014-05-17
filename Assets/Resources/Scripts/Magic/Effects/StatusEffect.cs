using UnityEngine;
using System.Collections;

public enum StatusType {Poison, Slow, ReducedDefence}

public class StatusEffect : Effect{
	public int TickCount {get; protected set;}
	public float Power {get; protected set;}
	public StatusType Status {get; protected set;}

	public StatusEffect(int tickCount, float power, StatusType se) : base(tickCount, power){
		Status = se;
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
		StatusEffect newStatus = (StatusEffect)obj;
		return this.Status == newStatus.Status;
	}
	
	public override int GetHashCode () {
		return Status.ToString().GetHashCode() << 16 | ((int)Power);
	}

}