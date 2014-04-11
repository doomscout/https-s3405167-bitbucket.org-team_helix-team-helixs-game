using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	//teleports GameObject to desired tile.
	public void tileJump(Vector3 direction){
		transform.position = direction;
	}

	//gonna have to change the movement here and in Player.cs to match with the tile system.
}
