using UnityEngine;
using System.Collections;

public enum WorldType {Wall, Mine}

public class WorldEffect : Effect {
	public int TickCount {get; protected set;}
	public float Power {get; protected set;}
	public WorldType WorldType {get; protected set;}
	
	public WorldEffect(int tickCount, float power, WorldType se) : base(tickCount, power) {
		WorldType = se;
	}

	public WorldEffect(WorldEffect e) : base(e.TickCount, e.Power)  {
		this.WorldType = e.WorldType;
	}
	
	public bool TickDown() {
		TickCount--;
		Power *= 2;
		if (TickCount <= 0) {
			return false;
		}
		return true;
	}
	
	public override bool Equals (object obj) {
		if (obj == null || this.GetType() != obj.GetType()) {
			return false;
		}
		WorldEffect newStatus = (WorldEffect)obj;
		return this.WorldType == newStatus.WorldType;
	}
	
	public override int GetHashCode () {
		return WorldType.ToString().GetHashCode() << 16 | ((int)Power);
	}
	
}