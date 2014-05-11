using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : Cleanable {
	//Stats
	public float Max_Health {get;set;}
	public float Health {get;set;}

	//Animation
	public bool FinishedAnimation{get;set;}
	public bool IsDead{get; protected set;}
	public float MoveSpeed{get;set;}
	protected float remainingDistance;
	protected Direction current_target;
	
	//Magic
	public Spell MainSpell;
	public Colour MainColour;
	public List<Status> ListStatus;
	public float[] TickedStatus;

	//Map
	public int Map_position_x{ get; protected set;}
	public int Map_position_y{ get; protected set;}

	//GameObject
	public GameObject game_object {get; protected set;}

	protected Entity() {
		//Stats
		InitStats();
		//Animation
		InitAnimation();
		//Magic
		InitMagic();
		//Map
		InitMapPosition();
		//GameObject
		InitGameObject();

		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	protected virtual void InitAnimation() {
		FinishedAnimation = false;
		IsDead = false;
		MoveSpeed = 10.0f;
		remainingDistance = 1.0f;
		current_target = Direction.None;
	}

	protected virtual void InitStats() {
		Max_Health = 10.0f;
		Health = Max_Health;
	}

	protected virtual void InitMagic() {
		MainColour = ColourManager.getRandomColour();
		MainSpell = new Spell("single", MainColour);
		ListStatus = new List<Status>();
		TickedStatus = new float[10];
	}
	
	public void CleanUp() {
		if (game_object != null) {
			GameObject.Destroy(game_object);
		}
	}

	public virtual void status_tick() {
		List<Status> toBeRemoved = new List<Status>();
		int count = ListStatus.Count;
		for (int i = 0; i < count; i++) {
			TickedStatus[(int)ListStatus[i].StatusEffect] = ListStatus[i].Power;
			ListStatus[i].TickDown();
			if (ListStatus[i].TickCount <= 0) {
				toBeRemoved.Add (ListStatus[i]);
			}
		}
		foreach (Status s in toBeRemoved) {
			ListStatus.Remove(s);
		}
	}

	public virtual void CastMainSpell() {
		if (TickedStatus[(int)StatusEffects.ReducedDamage] != 0) {
			MainSpell.SpellPowerModifier = TickedStatus[(int)StatusEffects.ReducedDamage];
		} else {
			MainSpell.SpellPowerModifier = 1.0f;
		}
	}

	public virtual float GetHitByMagic(Spell taken_spell) {
		float modifier = 1.0f;
		if (ColourManager.getWeakness(taken_spell.SpellColour) == MainColour) {
			//The spell is weak against our colour
			modifier = ColourManager.WeaknessModifier;
		}
		float dmg = taken_spell.Power * modifier * taken_spell.SpellPowerModifier;
		Health -= dmg;
		return dmg;
	}
	
	public virtual void death_tick() {
		//display death animation (if any)
		GameObject.Destroy(game_object);
		FinishedAnimation = true;			//temp no animation, just return immediately
	}
	
	protected abstract void InitMapPosition();
	protected abstract void InitGameObject();
	public abstract void animation_tick();
}
