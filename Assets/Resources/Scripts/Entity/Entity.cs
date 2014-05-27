using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : Cleanable {
	//general stuff
	public string name;

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
	public List<StatusEffect> ListStatus;
	private List<StatusEffect>[] TickedStatus;

	//Money
	public float Money;

	//Map
	public int Map_position_x{ get; protected set;}
	public int Map_position_y{ get; protected set;}

	//GameObject
	public GameObject game_object {get; protected set;}

	//ensare balance because its super borken
	private int EnsnareImmunity;

	protected Entity() {
		PreInit();
		InitAll();
	}

	protected Entity(int x, int y) {
		Map_position_x = x;
		Map_position_y = y;
		PreInit();
		InitAll();
	}

	private void InitAll() {
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

	protected virtual void PreInit() {
		name = "";
	}

	protected virtual void InitAnimation() {
		FinishedAnimation = false;
		MoveSpeed = 5.0f;
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
		ListStatus = new List<StatusEffect>();
		TickedStatus = new List<StatusEffect>[System.Enum.GetNames(typeof(StatusType)).Length];
		for (int i = 0; i < TickedStatus.Length; i++) {
			TickedStatus[i] = new List<StatusEffect>();

		}
	}

	protected virtual void InitMoney() {
		Money = Random.Range(3, 5);
	}
	
	public void CleanUp() {
		if (game_object != null) {
			GameObject.Destroy(game_object);
		}
	}

	public virtual void status_tick() {
		List<StatusEffect> toBeRemoved = new List<StatusEffect>();
		int count = ListStatus.Count;

		//Reset status list
		for (int i = 0; i < TickedStatus.Length; i++) {
			TickedStatus[i] = new List<StatusEffect>();
		}

		//Populate status list for this turn
		for (int i = 0; i < count; i++) {
			TickedStatus[(int)ListStatus[i].Status].Add(ListStatus[i]);
			Debug.Log ("Type: " + ListStatus[i].EffectName() + " Power: " + ListStatus[i].Power + " Tick " + ListStatus[i].TickCount);
			ListStatus[i].TickDown();
			if (ListStatus[i].TickCount <= 0) {
				toBeRemoved.Add (ListStatus[i]);
			}
		}

		//Remove status that have been timed out
		foreach (StatusEffect s in toBeRemoved) {
			if (!ListStatus.Remove(s)) {
				Debug.Log ("Can't remove");
			} else {
				Debug.Log ("Removed");
			}

		}
	}

	public virtual void Prelogic_tick() {
		float dmg = 0.0f;
		//populate all the statuses
		status_tick();
		// Status modifier (poison)
		for (int i = 0; i < TickedStatus[(int)StatusType.Poison].Count; i++) {
			if (TickedStatus[(int)StatusType.Poison][i].Status != StatusType.Poison) {
				Debug.LogError("Poison tick error");
			}
			dmg += TickedStatus[(int)StatusType.Poison][i].Power;
			BattleLog.GetInstance().AddMessage("[Turn " + GameTools.GI.NumberOfTurnsUntilWin +"] " + name + " poisoned for " + dmg + " damage.");
			ShowText("Poisoned " + dmg, Color.green, i - 2);
		}
		EnsnareImmunity--;
		Health -= dmg;
	}

	public virtual void CastMainSpell() {
		/* We can add in another status in here */
	}

	public virtual void ApplyStatusOnHit(Spell taken_spell) {
		/* Receive status effects */
		if (taken_spell.SpellEffect.TickCount > 0 && taken_spell.SpellEffect.GetType() == typeof(StatusEffect)) {
			StatusEffect se = (StatusEffect)taken_spell.SpellEffect;
			//Ugle if statements to check for the broken ensnare effect
			if (se.EffectName() == "Ensnare") {
				if (EnsnareImmunity <= 0) {
					EnsnareImmunity = 10;
					ListStatus.Add(new StatusEffect(se.TickCount, se.Power, se.Status));
				} 
			} else {
				ListStatus.Add(new StatusEffect(se.TickCount, se.Power, se.Status));
			}
		}
	}

	public virtual float GetHitByMagic(Spell taken_spell) {
		float modifier = 1.0f;
		ApplyStatusOnHit(taken_spell);
		// Colour modifier
		if (ColourManager.getWeakness(taken_spell.SpellColour) == MainColour) {
			//The spell is weak against our colour
			modifier = ColourManager.WeaknessModifier;
		}
		//status modifier (ensnare)
		// Entity will be unable to move unless the ensare is destroyed by magic
		// The ensare hp is its power
		if (TickedStatus[(int)StatusType.Ensnare].Count > 0) {
			TickedStatus[(int)StatusType.Ensnare][0].Power -= taken_spell.Power;
			if (TickedStatus[(int)StatusType.Ensnare][0].Power < 0) {
				TickedStatus[(int)StatusType.Ensnare].RemoveAt(0);
			}
		}

		float dmg = taken_spell.Power * modifier;
		return dmg;
	}

	public virtual bool animation_tick(){
		if (TickedStatus[(int)StatusType.Ensnare].Count > 0) {
			if (!FinishedAnimation) {
				ShowText("Ensnared: " + (TickedStatus[(int)StatusType.Ensnare][0].TickCount + 1), Color.white, -1);
			}
			return false;
		}
		return true;
	}

	public virtual bool IsDead() {
		return Health <= 0;
	}
	
	public virtual void death_tick() {
		//display death animation (if any)
		float errorpopupindebug = 0;
		GameObject.Destroy(game_object, 2.0f);
		FinishedAnimation = true;			//temp no animation, just return immediately
	}

	public virtual void OnClickAction() {}

	public virtual bool IsEntityAbleToMoveHere(Entity e) {
		return false;
	}

	protected void ShowText(string text, Color c, int offset) {
		GameObject o = Object.Instantiate(Resources.Load("Prefabs/DamagePopupPrefab", typeof(GameObject))) as GameObject;
		DamagePopup script = o.GetComponent<DamagePopup>();
		script.setText(text);
		script.setColor(c);
		o.transform.position = new Vector3(game_object.transform.position.x, 0, game_object.transform.position.z + 1.0f + offset/2.0f);
	}

	protected abstract void InitMapPosition();
	protected abstract void InitGameObject();
	protected abstract void InitCleanable();

}
