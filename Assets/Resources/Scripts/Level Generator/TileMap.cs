using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour {

	//Declare variable of Size of Map
	public int size_x = 100;
	public int size_z = 50;
	public float tileSize = 1.0f;
	public int percentAreTile =90;
	public Colour[,] store_data;

	public Unit[,] map_unit_occupy;
	public bool hasInit = false;

	//Declare variable of Texture and texture resolution
	public Texture2D terrainTiles;
	public int tileResolution;

	private DataTileMap map;


	// Use this for initialization
	void Start () {
		init();
	}

	public void init() {
		if (!hasInit) {
			BuildMesh();

			map_unit_occupy = new Unit[size_x,size_z];			
			this.transform.Translate(0, 0, size_z);
			this.transform.Translate(-0.5f, 0f, -0.5f);
			GameTools.Map = this;
			hasInit = true;
		}
	}

	public void cleanUp() {
		GameTools.Map = null;
		TileMouseOver tileMouseOverScript = gameObject.GetComponent<TileMouseOver>();
		tileMouseOverScript.cleanUp();
		gameObject.transform.position = new Vector3(-100, -100, -100);
		Object.Destroy(gameObject);
		Debug.Log ("TileMap cleaned up");
	}

	//import and read image file and chop down each color for each tile.

	Color[][] ChopUpTiles() {
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;
		
		Color[][] tiles = new Color[numTilesPerRow*numRows][];
		
		for(int y=0; y<numRows; y++) {
			for(int x=0; x<numTilesPerRow; x++) {
				tiles[y*numTilesPerRow + x] = terrainTiles.GetPixels( x*tileResolution , y*tileResolution, tileResolution, tileResolution );
			}
		}
		
		return tiles;
	}

	//import and read image file and chop down each color for each tile.

	void BuildTexture(DataTileMap bestMap) {
		map = bestMap;
		
		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);
		
		Color[][] tiles = ChopUpTiles();

		for(int y=0; y < size_z; y++) {
			for(int x=0; x < size_x; x++) {
				Color[] p = tiles[ map.GetTileAt(x,y) ];
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
			}
		}
		
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		
		Debug.Log ("Done Texture!");

		store_data = new Colour[size_x, size_z];
		for(int y = 0; y < size_z; y++) {
			for(int x = 0; x < size_x; x++) {
				store_data[x, y] = map.Map_data[x,y];
				//Debug.Log("col = " + (x+1) +" row = "+ (y+1) + " ColorNumber "+ store_data[x, y]); 
			}
		}


	}
	
	public void BuildMesh() {


		int numTiles = size_x * size_z;
		int numTris = numTiles * 2;
		
		int vsize_x = size_x + 1;
		int vsize_z = size_z + 1;
		int numVerts = vsize_x * vsize_z;
		
		// Generate the mesh data
		Vector3[] vertices = new Vector3[ numVerts ];
		Vector3[] normals = new Vector3[numVerts];
		Vector2[] uv = new Vector2[numVerts];
		
		int[] triangles = new int[ numTris * 3 ];
		
		int x, z;
		for(z=0; z < vsize_z; z++) {
			for(x=0; x < vsize_x; x++) {
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, -z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / size_x, 1f - (float)z / size_z );
			}
		}
		Debug.Log ("Done Verts!");
		
		for(z=0; z < size_z; z++) {
			for(x=0; x < size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 5] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 4] = z * vsize_x + x + 		   1;
			}
		}
		
		Debug.Log ("Done Triangles!");
		
		// Create a new Mesh and populate with the data
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		
		// Assign our mesh to our filter/renderer/collider
		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");

		//Generate and test maps
		List<DataTileMap> list_of_generated_maps = new List<DataTileMap>();
		for (int i = 0; i < 10; i++) {
			DataTileMap newMap = new DataTileMap(size_x, size_z, percentAreTile);
			list_of_generated_maps.Add(newMap);
		}
		DataTileMap bestMap = testMaps(list_of_generated_maps);
		BuildTexture(bestMap);

		//PrintDebug();
	}

	private DataTileMap testMaps(List<DataTileMap> list) {
		DataTileMap bestMap = null;
		int maxNum = 0;

		for (int i = 0; i < list.Count; i++) {
			if (!list[i].isAllsConnected()) {
				continue;
			}
			if (bestMap == null) {
				bestMap = list[i];
				maxNum = list[i].numberOfColourTiles;
			} else {
				if (list[i].numberOfColourTiles > maxNum) {
					bestMap = list[i];
					maxNum = list[i].numberOfColourTiles;
				}
			}
		}
		if (bestMap == null) {
			Debug.LogError("Looked at 10 maps, none of them are any good");
		}
		return bestMap;
	}

	public bool isOutOfBounds(int x, int y) {
		return 	x < 0 || x >= size_x ||
				y < 0 || y >= size_z;
	}
	
}



/*
		vertices[0] = new Vector3(0,0,0);
		vertices[1] = new Vector3(1,0,0);
		vertices[2] = new Vector3(0,0,-1);
		vertices[3] = new Vector3(1,0,-1);

		triangles[0] = 0;
		triangles[1] = 3;
		triangles[2] = 2;

		triangles[3] = 0;
		triangles[4] = 1;
		triangles[5] = 3;

		normals[0] = Vector3.up;
		normals[1] = Vector3.up;
		normals[2] = Vector3.up;
		normals[3] = Vector3.up;

		uv[0] = new Vector2(0,1);
		uv[0] = new Vector2(1,1);
		uv[0] = new Vector2(0,0);
		uv[0] = new Vector2(1,0);

	Random.Range (-0.1f, 0.1f)

 */
