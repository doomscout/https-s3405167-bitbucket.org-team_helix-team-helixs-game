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
		
		TileMouseOver selector = new TileMouseOver();
		Vector3 selection = selector.getSelector();
		int[] mouse = new int[2];
		mouse[0] = Mathf.CeilToInt(selection.x);
		mouse[1] = Mathf.CeilToInt(selection.y);

		int[][] coordinates = shape.shapeSpell(centre, mouse, aoe);

		//deal damage to anything in those tiles
	}

}
