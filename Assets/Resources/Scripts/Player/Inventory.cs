using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory {
	public List<Spell> spellInventory = new List<Spell>();

	public Inventory() {

	}

	public void addSpell() {

	}

	public void removeSpell() {

	}

	public void generateSpell() {
		spellInventory.Add(new Spell());
	}
}

public class Deck {
	public List<Spell> spellDeck = new List<Spell>();

	public Deck() {
		for(int i = 0; i < 6; i++) {
			spellDeck.Add(new Spell());
		}
	}

	public void dropSpell(int element) {
		spellDeck.RemoveAt(element);
	}

	public void addSpell() {
		
	}
	
	public void removeSpell() {
		
	}
}

public class DeckManager {
	Inventory inv = new Inventory();
	Deck deck = new Deck();

	public void addToDeck() {
		deck.addSpell();
		inv.removeSpell();
	}

	public void removeFromDeck() {
		inv.addSpell();
		deck.removeSpell();
	}
}