using UnityEngine;
using System.Collections;

public class Spell {
	Shape shape = new Shape();

	public Spell () {
		float dmg = Random.Range(1, 50);
		int c = (int)Random.Range (1, 6);

		shape.setShape();
		string s = shape.getShape();

		Debug.Log (dmg + ", " + c + ", " + s + "\n");
	}

}
