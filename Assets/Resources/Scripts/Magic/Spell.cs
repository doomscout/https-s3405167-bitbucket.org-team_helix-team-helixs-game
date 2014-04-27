using UnityEngine;
using System.Collections;

public class Spell {
	public Shape Shape {get; private set;}
	public Colour SpellColour {get; private set;}
	public float Power {get; private set;}

	public Spell () {
		SpellColour = ColourManager.getRandomColour();
		Power = Random.Range(1, 50);
		Shape = new Shape();

	}

	public Spell (string shape) : this() {
		Shape = new Shape(shape);
	}

	public Spell (Colour c) : this() {
		SpellColour = c;
	}

	public Spell (string shape, Colour c) : this(shape) {
		SpellColour = c;
	}

	public void cast(int[] origin, int[] position) {
		int[,] coordinates = Shape.toCoords(origin, position);

		for (int i = 0; i < coordinates.GetLength(0); i++) {
			if (GameTools.Map.isOutOfBounds(coordinates[i,0], coordinates[i,1])) {
				continue;
			}
			if (GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]] != null) {
				GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]].getHitByMagic(this);
			}
			if (coordinates[i,0] == GameTools.Player.Map_position_x && coordinates[i,1] == GameTools.Player.Map_position_y) {
				GameTools.Player.getHitByMagic(this);
			}

		}
	}

}
