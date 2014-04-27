using UnityEngine;
using System.Collections;


[RequireComponent(typeof(TileMap))]

	public class TileMouseOver : MonoBehaviour 
	{

	public Color highlightColor;
	Color normalColor;

	TileMap _tileMap;
	Colour colorNumber;
	Vector3 currentTileCoord;

	public Transform selectionCube;
	// Use this for initialization
	void Start () {
		_tileMap = GetComponent<TileMap>();
	}

	void Update()
	{
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if(collider.Raycast(ray, out hitInfo, Mathf.Infinity ) )
		{

			//Debug.Log (hitInfo.point - transform.point);
			int x = Mathf.RoundToInt ( hitInfo.point.x / _tileMap.tileSize);
			int z = Mathf.RoundToInt ( hitInfo.point.z / _tileMap.tileSize);
			colorNumber = GameTools.Map.store_data[x, z];

			Debug.Log ("Map Position: (" + x + ", " + z + ") Color: " + colorNumber);

			currentTileCoord.x = x - 0.5f;
			currentTileCoord.z = z - 0.5f;

			selectionCube.transform.position = currentTileCoord;

		}
		else 
		{

		}

		if(Input.GetMouseButtonDown(0))
		 {
			Debug.Log ("Click!");
		}
	}

	public Vector3 getSelector() {
		return currentTileCoord;
	}
	
	/*
	// Update is called once per frame
	void OnMouseOver(){
		renderer.material.color = Color.blue;
	}

	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}
	*/
}
