using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
	Shape shape = new Shape();
	public Colour SpellColour {get; private set;}
	public float Power {get; private set;}

	public Spell () {
		Power = Random.Range(1, 50);
		SpellColour = ColourManager.getRandomColour();

		shape.setShape();
		string aoe = shape.getShape();

		Debug.Log (Power + ", " + SpellColour + ", " + aoe + "\n");
	}

	public static int[] getPlayerPosition() {
		int[] position = new int[2];
		position[0] = GameTools.Player.Map_position_x;
		position[1] = GameTools.Player.Map_position_y;
		
		return position;
	}

	public static int[] getMousePosition() {
		TileMouseOver selector = GameTools.Mouse;
		int[] mouse = new int[2];
		mouse[0] = selector.Pos_x;
		mouse[1] = selector.Pos_z;

		return mouse;
	}

	public void cast(string aoe) {
		int[] centre = getPlayerPosition ();
		int[] mouse = getMousePosition();

		int[,] coordinates = shape.shapeSpell(centre, mouse, aoe);

		for (int i = 0; i < coordinates.GetLength(0); i++) {
			if (GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]] != null) {
				GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]].takeDamage(this);
			}

		}

		//deal damage to anything in those tiles
	}

}
