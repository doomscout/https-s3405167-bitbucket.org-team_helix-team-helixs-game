using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellGenerator {

	private static SpellGenerator instance;
	private List<Spell> GeneratedSpells;
	private int SpellLimit = 200;

	private SpellGenerator() {
		GeneratedSpells = new List<Spell>();
		RefreshList();
	}

	public static SpellGenerator GetInstance(){
		if (instance == null) {
			instance = new SpellGenerator();
		}
		return instance;
	}

	public void RefreshList() {
		GeneratedSpells = new List<Spell>();
		for (int i = 0; i < SpellLimit; i++) {
			GeneratedSpells.Add (new Spell());
		}
		GeneratedSpells.Sort(new SpellComparer());
	}

	public Spell GetClosestSpell(int rating) {
		float minRating = Mathf.Abs(rating - GeneratedSpells[0].SpellRating);
		Spell returnSpell = GeneratedSpells[0];
		for (int i = 0; i < GeneratedSpells.Count; i++) {
			if (Mathf.Abs(rating - GeneratedSpells[i].SpellRating) < minRating) {
				minRating = Mathf.Abs(rating - GeneratedSpells[i].SpellRating);
				returnSpell = GeneratedSpells[i];
			}
		}
		GeneratedSpells.Remove(returnSpell);
		return returnSpell;
	}

	public void PrintAllSpells() {
		for (int i = 0; i < GeneratedSpells.Count; i++) {
			Debug.Log ("Spell " + i + ": " + GeneratedSpells[i].SpellRating);
		}
	}
}
