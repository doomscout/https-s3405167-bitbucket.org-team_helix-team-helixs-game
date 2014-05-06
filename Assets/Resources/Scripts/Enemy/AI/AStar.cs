using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Singleton AStar implementation with lazy loading.
public class AStar {

	private static AStar instance = null;
	private static int x;
	private static int y;
	private static float heavyWeight = 250.0f;

	private AStar() {

	}

	public static AStar fromPosition(int x1, int y1) {
		if (instance == null) {
			instance = new AStar();
		}
		x = x1;
		y = y1;
		return instance;
	}

	public Stack<Direction> findPathToPostion(int destinationX, int destinationY) {
		IComparer comparer = new AStarComparer();
		Heap<AStarNode> openSet = new Heap<AStarNode>(comparer);
		HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
		bool found = false;
		int debugCount = 0;	
		
		AStarNode nodeCurrentPosition = new AStarNode(x, y, 0.0f, chessboardDistanceFromTarget(destinationX, destinationX), Direction.None);
		AStarNode nodeDestination = new AStarNode(destinationX, destinationY, 0.0f, 0.0f, Direction.None);
		
		openSet.insert(nodeCurrentPosition);
		
		while (openSet.length() > 0) {
			debugCount++;
			if (debugCount > 1000) {
				Debug.LogError("findPath in infinite (or just very big) loop");
				break;
			}
			nodeCurrentPosition = openSet.extract();
			closedSet.Add(nodeCurrentPosition);
			
			if (closedSet.Contains(nodeDestination)) {
				//found the optimal path
				found = true;
				break;
			}
			
			List<AStarNode> listOfNeighbours = findNeighbours(nodeCurrentPosition);
			
			foreach (AStarNode node in listOfNeighbours) {
				if(closedSet.Contains(node)) {
					//we've already explored this node, so let's ignore it
				} else {
					if (openSet.contains(node)) {
						AStarNode temp = openSet.getItem(node);
						if (comparer.Compare(temp, node) > 0) {
							//New node is better than old, so let's replace it
							if (openSet.remove(temp)) {	
							}//The hash values are the same, but they're not the same object.
							openSet.insert(node);	//ugh, what a crappy implementation
						}
					} else {
						openSet.insert(node);
					}
				}
			}
		}
		
		Stack<Direction> path = new Stack<Direction>();
		if (!found) {
			//Debug.Log ("(" + x + ", " + y + ") found nothing");
		} else {
			AStarNode pointer = nodeCurrentPosition;
			int count = 0;
			while (pointer != null) {
				path.Push(pointer.d);
				pointer = pointer.Prev;
				count++;
				if (count > 100) {
					Debug.LogError("Count over 100");
				}
			}
		}
		return path;
	}

	//This function is also used in simpleAI
	public float euclidianDistanceFromTarget(float target_x, float target_y) {
		float ans = Mathf.Sqrt((target_x - x) * (target_x - x) + 
		                       (target_y - y) * (target_y - y));
		return ans;
	}

	public float manhattanDistanceFromTarget(float target_x, float target_y) {
		return Mathf.Abs(x - target_x) + Mathf.Abs(y - target_y);
	}

	public float chessboardDistanceFromTarget(float target_x, float target_y) {
		float ans = Mathf.Max(target_x, target_y);
		return ans;
	}
	
	private List<AStarNode> findNeighbours(AStarNode a) {
		List<AStarNode> listOfNeighbours = new List<AStarNode>();
		float weight = 1.0f;	//default weight
		float distanceFromPlayer = 0.0f;
		int node_x = a.CoOrds[0], node_y = a.CoOrds[1];
		AStarNode newNode;
		
		//Logic to find valid neighbours
		weight = 1.0f;
		distanceFromPlayer = chessboardDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) / 5.0f;
		if (node_x >= 0 && node_x + 1 < GameTools.Map.size_x && node_y >= 0 && node_y + 1 < GameTools.Map.size_z ) {
			if (node_x - 1 >= 0 && GameTools.Map.store_data[node_x - 1, node_y] != Colour.None) {
				if (GameTools.Map.map_unit_occupy[node_x - 1, node_y] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(node_x - 1, node_y, a.getFScore() + weight, chessboardDistanceFromTarget(node_x - 1, node_y), Direction.Left);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
			if ( node_x + 1 < GameTools.Map.size_x && GameTools.Map.store_data[node_x + 1, node_y] != Colour.None) {
				weight = 1.0f;
				if (GameTools.Map.map_unit_occupy[node_x + 1, node_y] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(node_x + 1, node_y, a.getFScore() + weight, chessboardDistanceFromTarget(node_x + 1, node_y), Direction.Right);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
			
			weight = 1.0f;
			if (node_y - 1 >= 0 && GameTools.Map.store_data[node_x, node_y -1] != Colour.None) {
				if (GameTools.Map.map_unit_occupy[node_x, node_y - 1] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(node_x, node_y - 1, a.getFScore() + weight, chessboardDistanceFromTarget(node_x, node_y - 1), Direction.Down);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
			
			weight = 1.0f;
			if (node_y + 1 < GameTools.Map.size_z && GameTools.Map.store_data[node_x, node_y+1] != Colour.None) {
				if (GameTools.Map.map_unit_occupy[node_x, node_y + 1] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(node_x, node_y + 1, a.getFScore() + weight, chessboardDistanceFromTarget(node_x, node_y + 1), Direction.Up);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
		}
		return listOfNeighbours;
	}

	
	/* Debug methods */
	public void printFreeSpots() {
		int asd = 0;
		for (int i = 0; i < GameTools.Map.size_x; i++) {
			for (int j = 0; j < GameTools.Map.size_z; j++) {
				if (GameTools.Map.map_unit_occupy[i,j] != null) {
					asd++;
				}
			}
		}
		Debug.Log ("Bool count" + asd);
	}
	
	//if (x - 1 >= 0 && GameTools.Map.map_unit_occupy[x - 1, y] != true /*GameTools.Map.map.Map_data[x-1, y] != 0*/) {
	//if (x + 1 < GameTools.Map.size_x && GameTools.Map.map_unit_occupy[x + 1, y] != true /*x + 1 < GameTools.Map.size_x && GameTools.Map.map.Map_data[x+1, y] != 0*/) {
	//if (y - 1 >= 0 && GameTools.Map.map_unit_occupy[x, y - 1] != true /*y - 1 >= 0 && GameTools.Map.map.Map_data[x, y-1] != 0*/) {
	//if (y + 1 < GameTools.Map.size_z && GameTools.Map.map_unit_occupy[x, y + 1] != true/*y + 1 < GameTools.Map.size_z && GameTools.Map.map.Map_data[x, y+1] != 0*/) {


}
