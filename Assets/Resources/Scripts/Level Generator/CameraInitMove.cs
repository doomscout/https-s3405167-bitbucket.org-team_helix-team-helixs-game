using UnityEngine;
using System.Collections;

public class CameraInitMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (TileMap.size_x / 2.0f, TileMap.size_x>TileMap.size_z?TileMap.size_x:TileMap.size_z/1.2f, TileMap.size_z / 2.0f);
		this.transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
