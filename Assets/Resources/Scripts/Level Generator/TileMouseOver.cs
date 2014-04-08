using UnityEngine;
using System.Collections;


[RequireComponent(typeof(TileMap))]

	public class TileMouseOver : MonoBehaviour 
	{
	public Color highlightColor;
	Color normalColor;

	TileMap _tileMap;

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

			int x = Mathf.FloorToInt ( hitInfo.point.x / _tileMap.tileSize);
			int z = Mathf.FloorToInt ( hitInfo.point.z / _tileMap.tileSize);

			Debug.Log ("Tile :" + x + ", " + z);

			currentTileCoord.x = x;
			currentTileCoord.z = z;

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
