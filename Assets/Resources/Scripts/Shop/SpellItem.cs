using UnityEngine;
using System.Collections;

public class SpellItem {

	public Spell s;
	public float price;

	public SpellItem() {
		s = new Spell();
		price = Random.Range(1, 10);
	}

	public override bool Equals (object obj) {
		if (obj == null || this.GetType() != obj.GetType()) {
			return false;
		}
		SpellItem sp = (SpellItem)obj;
		return this.s == sp.s && this.price == sp.price;
	}
}
