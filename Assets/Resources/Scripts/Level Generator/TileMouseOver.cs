using UnityEngine;
using System.Collections;


[RequireComponent(typeof(TileMap))]

	public class TileMouseOver : MonoBehaviour, Cleanable 
	{

	public Color highlightColor;
	Color normalColor;

	TileMap _tileMap;
	Colour colorNumber;
	Vector3 currentTileCoord;

	public Transform selectionCube;
    //Pos_x and Pos_z to get the position in logic map (not worldspace coordinates)
	public int Pos_x {get;set;}
	public int Pos_z {get;set;}
	public bool IsOnMap {get;set;}

	// Use this for initialization
	void Start () {
		_tileMap = GetComponent<TileMap>();
		GameTools.Mouse = this;
		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	void Update()
	{
		if (GameTools.Map != null) {
					
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
		if (HasClickedOnUnit()) {
			if (GameTools.Map != null) {
				Unit u = GameTools.Map.map_unit_occupy[GameTools.Mouse.Pos_x, GameTools.Mouse.Pos_z];
				u.OnClickAction();
			}
		}
	}

	public void CleanUp() {
		if (gameObject == null) {
			return;
		}
		GameTools.Mouse = null;
		Object.Destroy(gameObject);
	}

	public Vector3 getSelector() {
		return currentTileCoord;
	}

	public bool HasClickedOnUnit() {
		return 	Input.GetMouseButtonUp(0) && 	
		    	IsOnMap &&
		    	GameTools.Map != null &&
				GameTools.Map.map_unit_occupy[GameTools.Mouse.Pos_x, GameTools.Mouse.Pos_z] != null;
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
