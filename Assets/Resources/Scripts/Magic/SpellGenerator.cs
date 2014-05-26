using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellGenerator {

	private static SpellGenerator instance;
	private List<Spell> GeneratedSpells;
	private List<Spell> GeneratedSingleSpells;
	private int SpellLimit = 200;

	private int MaxTimeToRefresh;
	private int TimeToRefresh;

	private SpellGenerator() {
		GeneratedSpells = new List<Spell>();
		GeneratedSingleSpells = new List<Spell>();
		MaxTimeToRefresh = 10;
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
		GeneratedSingleSpells = new List<Spell>();
		for (int i = 0; i < SpellLimit; i++) {
			GeneratedSpells.Add (new Spell());
		}
		for (int i = 0; i < SpellLimit; i++) {
			GeneratedSingleSpells.Add (new Spell(ShapeType.Single));
		}
		GeneratedSpells.Sort(new SpellComparer());
		GeneratedSingleSpells.Sort(new SpellComparer());
	}

	public Spell GetClosestSpell(int rating) {
		float minRating = Mathf.Abs(rating - GeneratedSpells[0].SpellRating);
		if (TimeToRefresh <= 0) {
			RefreshList();
		} else {
			TimeToRefresh--;
		}
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

	public Spell GetClosestSingleSpell(int rating) {
		float minRating = Mathf.Abs(rating - GeneratedSingleSpells[0].SpellRating);
		if (TimeToRefresh <= 0) {
			RefreshList();
		} else {
			TimeToRefresh--;
		}
		Spell returnSpell = GeneratedSingleSpells[0];
		for (int i = 0; i < GeneratedSingleSpells.Count; i++) {
			if (Mathf.Abs(rating - GeneratedSingleSpells[i].SpellRating) < minRating) {
				minRating = Mathf.Abs(rating - GeneratedSingleSpells[i].SpellRating);
				returnSpell = GeneratedSingleSpells[i];
			}
		}
		GeneratedSingleSpells.Remove(returnSpell);
		return returnSpell;
	}

	public void PrintAllSpells() {
		for (int i = 0; i < GeneratedSpells.Count; i++) {
			Debug.Log ("Spell " + i + ": " + GeneratedSingleSpells[i].SpellRating);
		}
	}
}
