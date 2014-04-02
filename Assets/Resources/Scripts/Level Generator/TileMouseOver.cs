using UnityEngine;
using System.Collections;

public class TileMouseOver : MonoBehaviour {
	public Color highlightColor;
	Color normalColor;
	// Use this for initialization
	void Start () {
		renderer.material.color = normalColor;
	}

	void Update(){
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
	

		RaycastHit hitInfo;
		if(collider.Raycast(ray, out hitInfo, Mathf.Infinity ) )
		{
			renderer.material.color = highlightColor;
		}
		else 
		{
			renderer.material.color = normalColor;
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
