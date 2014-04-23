using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	public void createSpell() {

		float ran = Random.Range(0, 6);

		//Colour spell_col = Colour(ran);

		Debug.Log (ran);

	}

}
