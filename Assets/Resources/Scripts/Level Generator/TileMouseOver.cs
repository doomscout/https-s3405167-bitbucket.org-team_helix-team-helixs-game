﻿using UnityEngine;
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
	public int Pos_x {get;set;}
	public int Pos_z {get;set;}
	public bool IsOnMap {get;set;}

	// Use this for initialization
	void Start () {
		_tileMap = GetComponent<TileMap>();
		GameTools.Mouse = this;
	}

	void Update()
	{
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if(collider.Raycast(ray, out hitInfo, Mathf.Infinity ) )
		{
			IsOnMap = true;
			//Debug.Log (hitInfo.point - transform.point);
			Pos_x = Mathf.RoundToInt ( hitInfo.point.x / _tileMap.tileSize);
			Pos_z = Mathf.RoundToInt ( hitInfo.point.z / _tileMap.tileSize);
			colorNumber = GameTools.Map.store_data[Pos_x, Pos_z];

			//Debug.Log ("Map Position: (" + Pos_x + ", " + Pos_z + ") Color: " + colorNumber);

			currentTileCoord.x = Pos_x - 0.5f;
			currentTileCoord.z = Pos_z - 0.5f;

			//selectionCube.transform.position = currentTileCoord;

		}
		else 
		{
			IsOnMap = false;
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
