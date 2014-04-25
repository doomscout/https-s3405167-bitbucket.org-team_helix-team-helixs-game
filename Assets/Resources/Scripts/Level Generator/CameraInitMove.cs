using UnityEngine;
using System.Collections;

public class CameraInitMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 pos = new Vector3 (GameTools.Map.size_x / 2.0f, GameTools.Map.size_x>GameTools.Map.size_z?GameTools.Map.size_x:GameTools.Map.size_z/1.2f, GameTools.Map.size_z / 2.0f);
		this.transform.position = pos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
