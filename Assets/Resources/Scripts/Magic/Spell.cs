using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {
	Shape shape = new Shape();

	public Spell () {
		float dmg = Random.Range(1, 50);
		int colour = (int)Random.Range (1, 6);

		shape.setShape();
		string aoe = shape.getShape();

		Debug.Log (dmg + ", " + colour + ", " + aoe + "\n");
	}

	public static int[] getPlayerPosition() {
		int[] position = new int[2];
		position[0] = GameTools.Player.Map_position_x;
		position[1] = GameTools.Player.Map_position_y;
		
		return position;
	}

	public void cast(string aoe) {
		int[] centre = getPlayerPosition ();
		
		TileMouseOver selector = GameTools.Mouse;
		int[] mouse = new int[2];
		mouse[0] = selector.Pos_x;
		mouse[1] = selector.Pos_z;

		int[,] coordinates = shape.shapeSpell(centre, mouse, aoe);

		for (int i = 0; i < coordinates.GetLength(0); i++) {
			GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
			o.transform.position = new Vector3(coordinates[i,0], 0, coordinates[i,1]);
		}

		//deal damage to anything in those tiles
	}

}
