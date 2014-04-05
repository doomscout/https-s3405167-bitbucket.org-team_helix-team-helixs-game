using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {

	float v = 0.5f;

	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		DrawDefaultInspector();


		if(GUILayout.Button ("Regenerate")){
			TileMap tileMap = (TileMap)target;
			tileMap.BuildMesh ();
		}

		
	}
}


/**
 * 
 * 
		EditorGUILayout.BeginVertical();
		EditorGUILayout.Slider(v, 0, 2.0f);
		EditorGUILayout.EndVertical();
	float v = 0.5f;
 */