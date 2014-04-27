using UnityEngine;
using System.Collections;

public class CameraInitMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GameTools.Player != null) {
			Vector3 pos = new Vector3 (GameTools.Player.player_object.transform.position.x, 8, GameTools.Player.player_object.transform.position.z);
			this.transform.position = pos;
		}
	}
}
