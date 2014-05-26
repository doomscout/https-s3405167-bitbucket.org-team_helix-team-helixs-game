using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class TileMap : MonoBehaviour, Cleanable {

	//Declare variable of Size of Map
	public int size_x;
	public int size_z;
	public float tileSize = 1.0f;
	public int percentAreTile;

	public Entity[,] map_unit_occupy;
	public bool hasInit = false;

	//Declare variable of Texture and texture resolution
	public Texture2D terrainTiles;
	public int tileResolution;

	private DataTileMap map;
	public TileType[,] TileMapData {get; private set;}
	public BonusTile[,] BonusTileData {get; private set;}
	public List<Trap>[,] TrapData {get; set;}
	public int[,] PassableMapData {get; private set;}
	public int[,] WeightedMap {get; private set;}
	public EnemySpawner[,] SpawnerMap {get;set;}
	public List<EnemySpawner> Spawners {get;set;}

	// Use this for initialization
	void Start () {
		init();
	}

	public void init() {
		if (!hasInit) {
			map_unit_occupy = new Unit[size_x,size_z];
			TileMapData = new TileType[size_x,size_z];
			WeightedMap =  new int[size_x, size_z];
			SpawnerMap = new EnemySpawner[size_x,size_z];
			Spawners = new List<EnemySpawner>();
			this.transform.Translate(0, 0, size_z);
			this.transform.Translate(-0.5f, 0f, -0.5f);
			GameTools.Map = this;
			CleanTools.GetInstance().SubscribeCleanable(this);
			hasInit = true;
			BuildMesh();
			GenerateBonusTiles();
			InitTrapMap();
		}
	}

	public void CleanUp() {
		if (gameObject == null) {
			return;
		}
		GameTools.Map = null;
		gameObject.transform.position = new Vector3(-100, -100, -100);
		Object.Destroy(gameObject);
		Debug.Log ("TileMap cleaned up");
	}

	private void GenerateBonusTiles() {
		BonusTileData = new BonusTile[size_x,size_z];

		for (int i = 0; i < size_x; i++) {
			for (int j = 0; j < size_z; j++) {
				if (TileTools.IsLand(TileMapData[i,j])) {
					if (TileMapData[i,j] == TileType.DarkGreen) {
						if (Random.Range (1,100) <= 5) {
							BonusTileData[i,j] = new BonusTile(i, j);
						}
					}
				}
			}
		}
	}

	private void InitTrapMap() {
		TrapData = new List<Trap>[size_x,size_z];
		for (int i = 0; i < size_x; i++) {
			for (int j = 0; j < size_z; j++) {
				TrapData[i,j] = new List<Trap>();
			}
		}
	}

	public void InitSpawners(int level) {
		int SpawnerCount = 10;

		while (SpawnerCount > 0) {
			int randX = Random.Range(0, size_x);
			int randY = Random.Range(0, size_z);
			if (GraphSearch.fromPosition(randX, randY)
			    	.manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) <= 10) {

				continue;
			}
			if (TileTools.IsLand(TileMapData[randX, randY])) {
				if (BonusTileData[randX, randY] == null && 
				    SpawnerMap[randX, randY] == null) {
					SpawnerCount--;
					EnemySpawner e = new EnemySpawner(randX, randY, level);
					SpawnerMap[randX, randY] = e;
					Spawners.Add(e);
				}
			}
		}
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

	void BuildTexture() {
		int texWidth = size_x * tileResolution;
		int texHeight = size_z * tileResolution;
		Texture2D texture = new Texture2D(texWidth, texHeight);

		Color[][] tiles = ChopUpTiles();

		for(int y=0; y < size_z; y++) {
			for(int x=0; x < size_x; x++) {
				int val = (int)TileMapData[x,y];
				if (val > 9) {
					Debug.LogError("Higher for some reason");
					val = 9;
				}
				Color[] p = tiles[ val ];
				texture.SetPixels(x*tileResolution, y*tileResolution, tileResolution, tileResolution, p);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
		mesh_renderer.sharedMaterials[0].mainTexture = texture;
		Debug.Log ("Done Texture!");
		return;

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

		DataTileMap bestMap1 = null;
		int debugCount = 0;

		while (bestMap1 == null) {
			debugCount++;
			if (debugCount > 2) {
				Debug.LogError("Infinite loop");
				break;
			}
			for (int i = 0; i < 10; i++) {
				DataTileMap newMap = new DataTileMap(size_x, size_z, percentAreTile);
				list_of_generated_maps.Add(newMap);
			}
			bestMap1 = testMaps(list_of_generated_maps);
		}

		bestMap1 = new DataTileMap(size_x, size_z, percentAreTile);
		GenerateHeightMap(bestMap1);
		BuildTexture();
		//PrintDebug();
	}

	public void UpdateWeightMap() {
		WeightedMap = (int[,])PassableMapData.Clone();
		GraphSearch	.fromPosition(GameTools.Base.Map_position_x, GameTools.Base.Map_position_y)
					.BreadthFirstFlood(WeightedMap, GraphSearch.DefaultPassable, GraphSearch.FillWeights);
	}

	private void GenerateHeightMap(DataTileMap map) {
		int[,] tempIntMap = (int[,])map.Map_data_passable.Clone();
		PassableMapData = (int[,])map.Map_data_passable.Clone();

		//Figure out which is ocrean water and which tiles is pool water
		int count = GraphSearch.fromPosition(0, 0).DepthFirstFlood(tempIntMap, new CheckPassable(GraphSearch.DefaultNotPassable), new ActionOnVisit(MarkAsOceanWater)).CountedTiles;
		for (int i = 0; i < size_x; i++) {
			for (int j = 0; j < size_z; j++) {
				if (tempIntMap[i,j] == -1) {
					TileMapData[i,j] = TileTools.OceanTile;
				} else if (tempIntMap[i,j] == 0) {
					TileMapData[i,j] = TileTools.PoolTile;
				}
				else if (tempIntMap[i,j] > 0) {
					TileMapData[i,j] = TileTools.InnerLandTile;
				}
			}
		}

		bool changed = false;
		int debugCount = 0;
		do {
			changed = false;
			debugCount++;
			if (debugCount > 1000) {
				Debug.LogError ("Infinite loop in here");
				return;
			}
			for (int i = 0; i < size_x; i++) {
				for (int j = 0; j < size_z; j++) {
					if (TileTools.IsLand(TileMapData[i,j])) {
						int newX;
						int newY;
						newX = i + 1;
						newY = j;
						if (!MapTools.IsOutOfBounds(newX, newY)) {
							if (TileTools.IsHigherByMoreThanOne(TileMapData[i,j], TileMapData[newX, newY])) {
								TileMapData[i,j] = TileTools.HeightMappingIncreaseTile(TileMapData[newX, newY]);
								changed = true;
							}
						}
						newX = i - 1;
						newY = j;
						if (!MapTools.IsOutOfBounds(newX, newY)) {
							if (TileTools.IsHigherByMoreThanOne(TileMapData[i,j], TileMapData[newX, newY])) {
								TileMapData[i,j] = TileTools.HeightMappingIncreaseTile(TileMapData[newX, newY]);
								changed = true;
							}
						}
						newX = i;
						newY = j + 1;
						if (!MapTools.IsOutOfBounds(newX, newY)) {
							if (TileTools.IsHigherByMoreThanOne(TileMapData[i,j], TileMapData[newX, newY])) {
								TileMapData[i,j] = TileTools.HeightMappingIncreaseTile(TileMapData[newX, newY]);
								changed = true;
							}
						}
						newX = i;
						newY = j - 1;
						if (!MapTools.IsOutOfBounds(newX, newY)) {
							if (TileTools.IsHigherByMoreThanOne(TileMapData[i,j], TileMapData[newX, newY])) {
								TileMapData[i,j] = TileTools.HeightMappingIncreaseTile(TileMapData[newX, newY]);
								changed = true;
							}
						}
					}
				}
			}
		} while (changed);

		Debug.Log ("HeightMap complete");
	}

	private void MarkAsOceanWater(int[,] map, GraphNode currNode) {
		map[currNode.x, currNode.y] = -1;
	}

	private DataTileMap testMaps(List<DataTileMap> list) {
		DataTileMap bestMap = null;
		int maxNum = 0;
		for (int i = 0; i < list.Count; i++) {
			/*
			int depthFirstCount = GraphSearch.fromPosition(list[i].xPosOfTile, list[i].yPosOfTile)
										.DepthFirstFlood(list[i].Map_data_passable, 0, GraphSearch.NoAction).CountedTiles;

			if (depthFirstCount != list[i].numberOfLandTiles) {
				continue;
			}
			*/
			if (bestMap == null) {
				bestMap = list[i];
				maxNum = list[i].numberOfLandTiles;
			} else {
				if (list[i].numberOfLandTiles > maxNum) {
					bestMap = list[i];
					maxNum = list[i].numberOfLandTiles;
				}
			}
		}
		if (bestMap == null) {
			Debug.Log("Looked at 10 maps, none of them are any good");
		}
		return bestMap;
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
