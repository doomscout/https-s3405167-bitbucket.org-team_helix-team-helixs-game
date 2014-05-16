using UnityEngine;
using System.Collections;

public class Spell {
	public Shape Shape {get; private set;}
	public Colour SpellColour {get; private set;}
	public float Power {get; private set;}
    public int CastRange {get; private set;}
	public StatusEffects StatusEffect {get; private set;}

    private int[] loadedOrigin;
	private int[] loadedDestination;
	private bool hasLoadInfo = false;

	public Spell () {
		SpellColour = ColourManager.getRandomColour();
		Shape = new Shape();
		Power = Random.Range(7, 10) /* Shape.shapeModifier*/;


		CastRange = Shape.CastRange;

	}

	public Spell (ShapeType shape) : this() {
		Shape = new Shape(shape);
		CastRange = Shape.CastRange;
	}

	public Spell (Colour c) : this() {
		SpellColour = c;
	}

	public Spell (ShapeType shape, Colour c) : this(shape) {
		SpellColour = c;
	}

	public void loadInfo(int[] origin, int[] position) {
		loadedOrigin = origin;
		loadedDestination = position;
		hasLoadInfo = true;
	}

	public void cast(int[] origin, int[] position) {
		int[,] coordinates = Shape.toCoords(origin, position);

		for (int i = 0; i < coordinates.GetLength(0); i++) {
			if (GameTools.Map.isOutOfBounds(coordinates[i,0], coordinates[i,1])) {
				continue;
			}
			if (GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]] != null) {
				GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]].GetHitByMagic(this);
			}
			if (coordinates[i,0] == GameTools.Player.Map_position_x && coordinates[i,1] == GameTools.Player.Map_position_y) {
				GameTools.Player.GetHitByMagic(this);
			}

		}
	}

	public bool cast() {
		if (hasLoadInfo) {
			cast(loadedOrigin, loadedDestination);
			return true;
		}
		Debug.LogError("Tried casting without loading info, this is bug");
		return false;
	}

}
