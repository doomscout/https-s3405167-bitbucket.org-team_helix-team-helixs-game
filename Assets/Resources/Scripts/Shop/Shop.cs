using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shop : Cleanable {

	public int SpellStockMaxLevel = 4;
	public List<SpellItem> SpellStock {get; private set;}

	public Shop() {
		RefreshStock();
		GameTools.Shop = this;
		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	public void RefreshStock() {
		SpellStock = new List<SpellItem>();
		for (int i = 0; i < SpellStockMaxLevel; i++) {
			SpellStock.Add(new SpellItem());
		}
	}

	public void ReplenishStock() {
		if (SpellStock == null) {
			RefreshStock();
			return;
		}
		int originalSpellLevel = SpellStock.Count;
		for (int i = 0; i < originalSpellLevel; i++) {
			SpellStock.Add(new SpellItem());
		}
	}

	public bool TryToBuySpell(Player p, SpellItem s) {
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

	public void CleanUp() {
		GameTools.Shop = null;
	}

}
