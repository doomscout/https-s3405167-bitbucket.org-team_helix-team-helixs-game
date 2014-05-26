
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : Cleanable {

	public int SpellStockMaxLevel = 10;
	public List<Spell> SpellStock {get; private set;}

	public Shop() {
		GameTools.Shop = this;
		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	public void RefreshStock(Player p) {
		int playerrating = p.CalculateLevel();
		float playermoney = p.Money;
		SpellStock = new List<Spell>();
		for (int i = 0; i < SpellStockMaxLevel; i++) {
			SpellStock.Add(SpellGenerator.GetInstance().GetClosestSpell(playerrating+(i*2)+((int)(playermoney/10.0f))));
		}
	}

	/* 
	public void ReplenishStock() {
		if (SpellStock == null) {
			RefreshStock();
			return;
		}
		int originalSpellLevel = SpellStock.Count;
		for (int i = 0; i < originalSpellLevel; i++) {
			SpellStock.Add(new Spell());
		}
	}
	*/

	//Sell a spell to player
	public bool TryToSellSpell(Player p, Spell s) {
		if (!SpellStock.Contains(s)) {
			Debug.Log ("Cannot find spell in shop");
			return false;
		}
		if (!p.BuySpell(s)) {
			Debug.Log ("Not enough funds to buy spell");
			return false;
		}
		SpellStock.Remove(s);
		//Everything is successful
		return true;
	}

	//Buy a spell from player
	public bool TryToBuySpell(Player p, Spell s) {

		return p.SellSpell(s);
	}

	public void CleanUp() {
		GameTools.Shop = null;
	}

}
