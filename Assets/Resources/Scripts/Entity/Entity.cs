using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : Cleanable {
	//Stats
	public float Max_Health {get;set;}
	public float Health {get;set;}

	//Animation
	public bool FinishedAnimation{get;set;}
	public float MoveSpeed{get;set;}
	protected float remainingDistance;
	protected Direction current_target;
	
	//Magic
	public Spell MainSpell;
	public Colour MainColour;
	public List<Status> ListStatus;
	private List<Status>[] TickedStatus;

	//Money
	public float Money;

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
		//Money
		InitMoney();
		//Map
		InitMapPosition();
		//GameObject
		InitGameObject();
		//InitCleanable
		InitCleanable();
	}

	protected virtual void InitAnimation() {
		FinishedAnimation = false;
		MoveSpeed = 10.0f;
		remainingDistance = 1.0f;
		current_target = Direction.None;
	}

	protected virtual void InitStats() {
		Max_Health = 1.0f;
		Health = Max_Health;
	}

	protected virtual void InitMagic() {
		MainColour = ColourManager.getRandomColour();
		MainSpell = new Spell(ShapeType.Single, MainColour);
		ListStatus = new List<Status>();
		TickedStatus = new List<Status>[System.Enum.GetNames(typeof(StatusEffects)).Length];
		for (int i = 0; i < TickedStatus.Length; i++) {
			TickedStatus[i] = new List<Status>();

		}
		//Sample status effect
		//ListStatus.Add(new PoisionStatus(3, 5.0f));				
		//ListStatus.Add(new PoisionStatus(3, 5.0f));
	}

	protected virtual void InitMoney() {
		Money = Random.Range(7, 12);
	}
	
	public void CleanUp() {
		if (game_object != null) {
			GameObject.Destroy(game_object);
		}
	}

	public virtual void status_tick() {
		List<Status> toBeRemoved = new List<Status>();
		int count = ListStatus.Count;

		//Reset status list
		for (int i = 0; i < TickedStatus.Length; i++) {
			TickedStatus[i] = new List<Status>();
		}

		//Populate status list for this turn
		for (int i = 0; i < count; i++) {
			TickedStatus[(int)ListStatus[i].StatusEffect].Add(ListStatus[i]);
			ListStatus[i].TickDown();
			if (ListStatus[i].TickCount <= 0) {
				toBeRemoved.Add (ListStatus[i]);
			}
		}

		//Remove status that have been timed out
		foreach (Status s in toBeRemoved) {
			ListStatus.Remove(s);
		}
	}

	public virtual void Prelogic_tick() {
		float dmg = 0.0f;
		//populate all the statuses
		status_tick();
		// Status modifier (poison)
		for (int i = 0; i < TickedStatus[(int)StatusEffects.Poison].Count; i++) {
			dmg += TickedStatus[(int)StatusEffects.Poison][i].Power;
		}
		Health -= dmg;
	}

	public virtual void CastMainSpell() {
		/* We can add in another status in here */
	}

	public virtual float GetHitByMagic(Spell taken_spell) {
		float modifier = 1.0f;
		// Colour modifier
		if (ColourManager.getWeakness(taken_spell.SpellColour) == MainColour) {
			//The spell is weak against our colour
			modifier = ColourManager.WeaknessModifier;
		}
		// Status modifier (reduced defence)
		for (int i = 0; i < TickedStatus[2].Count; i++) {
			modifier *= TickedStatus[(int)StatusEffects.ReducedDefence][i].Power;
		}

		float dmg = taken_spell.Power * modifier;
		Health -= dmg;
		return dmg;
	}

	public virtual bool IsDead() {
		return Health <= 0;
	}
	
	public virtual void death_tick() {
		//display death animation (if any)
		float death = 0;
		GameObject.Destroy(game_object);
		FinishedAnimation = true;			//temp no animation, just return immediately
	}
	
	protected abstract void InitMapPosition();
	protected abstract void InitGameObject();
	protected abstract void InitCleanable();
	public abstract void animation_tick();
}
