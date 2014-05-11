using UnityEngine;
using System.Collections;

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
	}
	
	public void CleanUp() {
		if (game_object != null) {
			GameObject.Destroy(game_object);
		}

	}

	public virtual float getHitByMagic(Spell taken_spell) {
		float modifier = 1.0f;
		if (ColourManager.getWeakness(taken_spell.SpellColour) == MainColour) {
			//The spell is weak against our colour
			modifier = ColourManager.WeaknessModifier;
		}
		float dmg = taken_spell.Power * modifier;
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
