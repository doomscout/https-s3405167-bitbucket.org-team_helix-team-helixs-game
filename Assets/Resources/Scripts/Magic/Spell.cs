using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
	public Stats stats;
	public Colour colour;
	public Shape shape;


	public Spell() {
		stats = new Stats();
		colour = new Colour();
		shape = new Shape();

		string spell_shape;
		string spell_col;
		float spell_dmg;

		shape.setShape();
		colour.setColour();
		stats.setDamage(10);

		spell_shape = shape.getShape();
		spell_col = colour.getColour();
		spell_dmg = stats.getDamage();

		Debug.Log ("Shape = " + spell_shape + "\n" +
		           "Colour = " + spell_col + "\n" +
		           "Damage = " + spell_dmg + "\n");

	}

}
