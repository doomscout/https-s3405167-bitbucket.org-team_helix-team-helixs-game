using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellComparer : Comparer<Spell> {

	public override int Compare (Spell x, Spell y) {
		return (int)((x.SpellRating - y.SpellRating) * 100);
	}
}
