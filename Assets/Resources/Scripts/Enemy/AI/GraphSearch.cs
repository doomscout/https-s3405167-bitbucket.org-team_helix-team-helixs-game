using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ActionOnVisit(int[,] graph, GraphNode currNode);
public delegate bool CheckPassable(int passable);
//Singleton AStar implementation with lazy loading.
public class GraphSearch {

	public static ActionOnVisit NoAction = new ActionOnVisit(Nothing);
	public static ActionOnVisit FillWeights = new ActionOnVisit(AddOneOnVisit);

	public static CheckPassable DefaultPassable = new CheckPassable(AboveZeroPassable);
	public static CheckPassable DefaultNotPassable = new CheckPassable(NonPassable);

	private static GraphSearch instance = null;
	private static int x;
	private static int y;

	//public int[,] WeightedMap {private get; private set;}
	public int CountedTiles {get; private set;}

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

	public GraphSearch BreadthFirstFlood(int[,] Map_data_passable, CheckPassable isPassable, ActionOnVisit action) {
		HelperBruteForce(Map_data_passable, isPassable, action, false);
		return instance;
	}

	public GraphSearch DepthFirstFlood(int[,] Map_data_passable, CheckPassable isPassable, ActionOnVisit action) {
		HelperBruteForce(Map_data_passable, isPassable, action, true);
		return instance;
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

	private static void Nothing(int[,] graph, GraphNode currNode) {

	}

	private static void AddOneOnVisit(int[,] graph, GraphNode currNode) {
		if (currNode.Prev != null) {
			graph[currNode.x, currNode.y] = graph[currNode.Prev.x, currNode.Prev.y] + 1; 
		} else {
			graph[currNode.x, currNode.y] = 0;
		}
	}

	private static bool AboveZeroPassable(int t) {
		return t > 0;
	}

	private static bool NonPassable(int t) {
		return !AboveZeroPassable(t);
	}

	private void HelperBruteForce(int[,] Map_data_passable, CheckPassable isPassable, ActionOnVisit action, bool isStack) {
		int count = 0;
		int newX = 0;
		int newY = 0;
		GraphNode neighbour;
		
		PriorityListWrapper<GraphNode> openSet = new PriorityListWrapper<GraphNode>(isStack);
		HashSet<GraphNode> closedSet = new HashSet<GraphNode>();
		
		GraphNode originNode = new GraphNode(x, y);
		openSet.Push(originNode);
		
		while (openSet.Count > 0) {
			GraphNode n = openSet.Pop();
			closedSet.Add(n);
			action(Map_data_passable, n);
			
			count++;
			if (count > 10000) {
				Debug.LogError("Infinite Loop");
				break;
			}
			
			newX = n.x+1;
			newY = n.y;
			neighbour = new GraphNode(newX, newY);
			neighbour.Prev = n;
			ValidNeighbour (Map_data_passable, isPassable, neighbour, openSet, closedSet);
			
			newX = n.x-1;
			newY = n.y;
			neighbour = new GraphNode(newX, newY);
			neighbour.Prev = n;
			ValidNeighbour (Map_data_passable, isPassable, neighbour, openSet, closedSet);
			
			newX = n.x;
			newY = n.y+1;
			neighbour = new GraphNode(newX, newY);
			neighbour.Prev = n;
			ValidNeighbour (Map_data_passable, isPassable, neighbour, openSet, closedSet);
			
			newX = n.x;
			newY = n.y-1;
			neighbour = new GraphNode(newX, newY);
			neighbour.Prev = n;
			ValidNeighbour (Map_data_passable, isPassable, neighbour, openSet, closedSet);
		}		
		CountedTiles = count;
	}

	private void ValidNeighbour(	int[,] Map_data_passable, 
	                              	CheckPassable isPassable, 
	                              	GraphNode neighbour, 
	                              	PriorityListWrapper<GraphNode> openSet, 
	                              	HashSet<GraphNode> closedSet) {
		if (!MapTools.IsOutOfBounds(neighbour.x, neighbour.y) && 
		    isPassable(Map_data_passable[neighbour.x, neighbour.y]) && 
		    !closedSet.Contains(neighbour) &&
		    !openSet.Contains(neighbour)) {
			openSet.Push (neighbour);
		}
	}
}
