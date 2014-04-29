using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : List<Spell> {

	public Inventory() {
		for(int i = 0; i < 10; i++) {
			this.Add(new Spell());
		}
	}

	public void generateNewSpell() {
		this.Add(new Spell());
	}
}

public class Deck : List<Spell> {
	private int count = 0;

	public Deck() {
		for(int i = 0; i < 10; i++) {
			this.Add(new Spell());
		}
	}

	public void shuffle() {

	}

	public Spell peekTopSpell() {
		return this[0];
	}

	public Spell popTopSpell() {
		//get top card and put at end
		Spell s = this[0];
		this.RemoveAt(0);
		this.Add (s);
		return s;
	}
}

public class ItemManager {
	Inventory inv = new Inventory();
	Deck deck = new Deck();

	public Spell peekTopSpell() {
		return deck.peekTopSpell();
	}

	public Spell popTopSpell() {
		return deck.popTopSpell();
	}
    public Spell getDeckSpell(int placement){
        return deck[placement];
    }
	public void moveSpellInventoryToDeck(Spell s) {
		if (!inv.Contains(s)) {
			return;
		}
		deck.Add(s);
		inv.Remove(s);
	}

	public void moveSpellDeckToInventory(Spell s) {
		if (!deck.Contains(s)) {
			return;
		}
		inv.Add(s);
		deck.Remove(s);
	}
}