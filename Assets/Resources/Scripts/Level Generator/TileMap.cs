using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour {
	
	static public int size_x = 10;
	static public int size_z = 20;
	public float tileSize = 1.0f;


	public Texture2D terrainTiles;
	public int tileResolution = 16;

	public int[,] map;

	// Use this for initialization
	void Start () {
		map = new int[size_z, size_x];
		BuildMesh();
		this.transform.position = new Vector3(-0.5f, 0.0f, -0.5f);
		GameTools.Map = map;
	}

	Color[][] ChopUpTiles(){
		//tileResolution = terrainTiles.height;
		int numTilesPerRow = terrainTiles.width / tileResolution;
		int numRows = terrainTiles.height / tileResolution;

		Color[][] tiles = new Color[numTilesPerRow*numRows][];

		for(int y = 0; y < numRows; y++){
			for(int x = 0; x < numTilesPerRow; x++){
				tiles[ y * numTilesPerRow + x] = terrainTiles.GetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}
		}

		return tiles;

	}

	void BuildTexture(){



		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		/*
		int texWidth = 10;
		int texHeight = 10;
		*/
		Texture2D texture = new Texture2D(texWidth, texHeight);

		Color[][] tiles = ChopUpTiles();

		for(int y = 0; y < size_z; y++){
			for(int x = 0; x < size_x; x++){
				int terrainTileoffset = Random.Range (0, 4) * tileResolution;
				Color[] p = terrainTiles.GetPixels(terrainTileoffset, 0, tileResolution, tileResolution);
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
				map[y,x] = terrainTileoffset/tileResolution;
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		Debug.Log ("Done Build Texture!");
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
				vertices[ z * vsize_x + x ] = new Vector3( x*tileSize, 0, z*tileSize );
				normals[ z * vsize_x + x ] = Vector3.up;
				uv[ z * vsize_x + x ] = new Vector2( (float)x / size_x, (float)z / size_z );
			}
		}
		Debug.Log ("Done Verts!");
		
		for(z=0; z < size_z; z++) {
			for(x=0; x < size_x; x++) {
				int squareIndex = z * size_x + x;
				int triOffset = squareIndex * 6;
				triangles[triOffset + 0] = z * vsize_x + x + 		   0;
				triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;
				triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;
				
				triangles[triOffset + 3] = z * vsize_x + x + 		   0;
				triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
				triangles[triOffset + 5] = z * vsize_x + x + 		   1;
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
		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();
		
		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("Done Mesh!");


		BuildTexture();

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
