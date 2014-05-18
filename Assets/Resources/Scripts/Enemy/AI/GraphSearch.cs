using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ActionOnVisit(int[,] graph, int x, int y);

//Singleton AStar implementation with lazy loading.
public class GraphSearch {

	public static ActionOnVisit NoAction = new ActionOnVisit(Nothing);

	private static GraphSearch instance = null;
	private static int x;
	private static int y;
	private static float heavyWeight = 250.0f;

	private GraphSearch() {

	}

	public static GraphSearch fromPosition(int x1, int y1) {
		if (instance == null) {
			instance = new GraphSearch();
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

	public int depthFirst(int[,] Map_data_passable, int impassableValue, ActionOnVisit action) {
		int count = 0;
		int newX = 0;
		int newY = 0;
		GraphNode neighbour;

		Stack<GraphNode> openSet = new Stack<GraphNode>();
		HashSet<GraphNode> closedSet = new HashSet<GraphNode>();
		
		GraphNode originNode = new GraphNode(x, y);
		openSet.Push(originNode);
		
		while (openSet.Count > 0) {
			GraphNode n = openSet.Pop();
			closedSet.Add(n);
			action(Map_data_passable, n.x, n.y);
			
			count++;
			if (count > 10000) {
				Debug.LogError("Infinite Loop");
				break;
			}
			
			newX = n.x+1;
			newY = n.y;
			neighbour = new GraphNode(newX, newY);
			if (!MapTools.IsOutOfBounds(newX, newY) && 
			    Map_data_passable[newX, newY] != impassableValue && 
			    !closedSet.Contains(neighbour) &&
			    !openSet.Contains(neighbour)) {
				openSet.Push (neighbour);
			}
			
			newX = n.x-1;
			newY = n.y;
			neighbour = new GraphNode(newX, newY);
			if (!MapTools.IsOutOfBounds(newX, newY) && 
			    Map_data_passable[newX, newY] != impassableValue && 
			    !closedSet.Contains(neighbour) &&
			    !openSet.Contains(neighbour)) {
				openSet.Push (neighbour);
			}
			
			newX = n.x;
			newY = n.y+1;
			neighbour = new GraphNode(newX, newY);
			if (!MapTools.IsOutOfBounds(newX, newY) && 
			    Map_data_passable[newX, newY] != impassableValue && 
			    !closedSet.Contains(neighbour) &&
			    !openSet.Contains(neighbour)) {
				openSet.Push (neighbour);
			}
			
			newX = n.x;
			newY = n.y-1;
			neighbour = new GraphNode(newX, newY);
			if (!MapTools.IsOutOfBounds(newX, newY) && 
			    Map_data_passable[newX, newY] != impassableValue && 
			    !closedSet.Contains(neighbour) &&
			    !openSet.Contains(neighbour)) {
				openSet.Push (neighbour);
			}
			
		}		
		return count;
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

	private static void Nothing(int[,] map, int x, int y) {

	}
	
	private List<AStarNode> findNeighbours(AStarNode a) {
		List<AStarNode> listOfNeighbours = new List<AStarNode>();
		float weight = 1.0f;	//default weight
		float distanceFromPlayer = 0.0f;
		int node_x = a.CoOrds[0], node_y = a.CoOrds[1];
		int newX = 0;
		int newY = 0;
		AStarNode newNode;
		
		//Logic to find valid neighbours
		weight = 1.0f;
		distanceFromPlayer = chessboardDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) / 5.0f;
		newX = node_x + 1;
		newY = node_y;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (TileTools.IsLand(GameTools.Map.TileMapData[newX, newY])) {
				weight = 1.0f;
				if (GameTools.Map.map_unit_occupy[newX, newY] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(newX, newY, a.getFScore() + weight, chessboardDistanceFromTarget(newX, newY), Direction.Right);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
		}
		newX = node_x - 1;
		newY = node_y;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (TileTools.IsLand(GameTools.Map.TileMapData[newX, newY])) {
				weight = 1.0f;
				if (GameTools.Map.map_unit_occupy[newX, newY] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(newX, newY, a.getFScore() + weight, chessboardDistanceFromTarget(newX, newY), Direction.Left);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
		}
		newX = node_x;
		newY = node_y + 1;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (TileTools.IsLand(GameTools.Map.TileMapData[newX, newY])) {
				weight = 1.0f;
				if (GameTools.Map.map_unit_occupy[newX, newY] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(newX, newY, a.getFScore() + weight, chessboardDistanceFromTarget(newX, newY), Direction.Up);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
		}
		newX = node_x;
		newY = node_y - 1;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (TileTools.IsLand(GameTools.Map.TileMapData[newX, newY])) {
				weight = 1.0f;
				if (GameTools.Map.map_unit_occupy[newX, newY] != null) {
					weight = heavyWeight/(distanceFromPlayer * distanceFromPlayer);
				}
				newNode = new AStarNode(newX, newY, a.getFScore() + weight, chessboardDistanceFromTarget(newX, newY), Direction.Down);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
		}
		
		return listOfNeighbours;
	}

	
	/* Debug methods */
	/*
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
	*/
}
