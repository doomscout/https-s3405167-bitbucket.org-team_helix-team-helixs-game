using UnityEngine;
using System.Collections;

public enum WorldType {Wall, Mine}

public class WorldEffect : Effect {
	public WorldType WorldType {get; protected set;}
	
	public WorldEffect(int tickCount, float power, WorldType se) : base(tickCount, power) {
		WorldType = se;
	}

	public WorldEffect(WorldEffect e) : base(e.TickCount, e.Power)  {
		this.WorldType = e.WorldType;
	}
	
	public override bool Equals (object obj) {
		if (obj == null || this.GetType() != obj.GetType()) {
			return false;
		}
		WorldEffect newStatus = (WorldEffect)obj;
		return this.WorldType == newStatus.WorldType && this.Power == newStatus.Power && TickCount == newStatus.TickCount;
	}
	
	public override int GetHashCode () {
		return WorldType.ToString().GetHashCode() << 16 | ((int)Power);
	}

	public override string EffectName () {
		return WorldType.ToString();
	}


}