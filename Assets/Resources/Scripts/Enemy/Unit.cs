using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {None, Up, Down, Left, Right};

public class Unit {
	public bool FinishedAnimation{get;set;}
	//public bool IsDead{get; private set;}
	public float MoveSpeed{get;set;}
	public float Health{get;private set;}
    public int Map_position_x { get; private set;}
    public int Map_position_y { get; private set;}
	GameObject unit;

	List<Direction> list_directions = new List<Direction>();
	Direction current_target = Direction.None;
	SimpleAI brain;
	float remainingDistance = 1f;


	public Unit() {
		brain = new SimpleAI(this);
		unit = Object.Instantiate(Resources.Load("Prefabs/EnemyPrefab", typeof(GameObject))) as GameObject;
		Map_position_x = Random.Range(1, 10);
		Map_position_y = Random.Range(1, 10);

		while (GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] != false ) {
			Map_position_x = Random.Range(1, 10);
			Map_position_y = Random.Range(1, 10);
		}

		GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = true;
        unit.transform.position = new Vector3(Map_position_x, 0, Map_position_y);
		Health = 10f;
		MoveSpeed = 10.0f;
	}

	public void printFreeSpots() {
		int asd = 0;
		for (int i = 0; i < GameTools.Map.size_x; i++) {
			for (int j = 0; j < GameTools.Map.size_z; j++) {
				if (GameTools.Map.map_unit_occupy[i,j]) {
					asd++;
				}
			}
		}
		Debug.Log ("Bool count" + asd);
	}

	public void determineNextMove() {
		Stack<Direction> stackOfDirections = findPath(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y);
		list_directions = new List<Direction>();
		if (stackOfDirections.Count == 0) {
			Debug.Log("StackOfDirections is 0");
			list_directions.Add(Direction.None);
			printFreeSpots();
			return;
		}
		Direction d = stackOfDirections.Pop();
		if (d == Direction.None) {
			Debug.Log ("Got the none direction");
			if (stackOfDirections.Count == 0) {
				Debug.Log("None direction and nothing else, something's wrong");
				return;
			}
			d = stackOfDirections.Pop ();
			if (d == Direction.None) {
				Debug.LogError("Two Nones in a row, something's wrong");
			} else {
				Debug.Log ("It works i think");
				//foreach (Direction a in stackOfDirections) {
				GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = false;
				int old_x = Map_position_x, old_y = Map_position_y;
				list_directions.Add (d);
				switch (d) {
				case Direction.Up:
					Map_position_y++;
					break;
				case Direction.Down:
					Map_position_y--;
					break;
				case Direction.Left: 
					Map_position_x--;
					break;
				case Direction.Right:
					Map_position_x++;
					break;
				}
				if (Map_position_x == GameTools.Player.Map_position_x && Map_position_y == GameTools.Player.Map_position_y) {
					Map_position_x = old_x;
					Map_position_y = old_y;
					list_directions.Remove(d);
				}
				foreach (Unit u in GameTools.All_Units) {
					if (u != this) {
						if (u.Map_position_x == Map_position_x && u.Map_position_y == Map_position_y) {
							Map_position_x = old_x;
							Map_position_y = old_y;
							list_directions.Remove(d);
							break;
						}
					}
				}
				GameTools.Map.map_unit_occupy[Map_position_x, Map_position_y] = true;


			}
		} else {
			Debug.LogError("No initial none direction, something's wrong");
		}
	}
	
	
	float distFromPlayer(float x, float y) {
		float ans = Mathf.Sqrt((GameTools.Player.Map_position_x - x) * (GameTools.Player.Map_position_x - x) + 
		                       (GameTools.Player.Map_position_y - y) * (GameTools.Player.Map_position_y - y));
		return ans;
	}

	List<AStarNode> findNeighbours(AStarNode a) {
		List<AStarNode> listOfNeighbours = new List<AStarNode>();
		float weight = 1.0f;	//let weight be 1 for now
		int x = a.CoOrds[0], y = a.CoOrds[1];
		AStarNode newNode;

		weight = 1.0f;
		if (x >= 0 && x + 1 < GameTools.Map.size_x && y >= 0 && y + 1 < GameTools.Map.size_z ) {
			if (x - 1 >= 0 ) {
				if (GameTools.Map.map_unit_occupy[x - 1, y]) {
					weight = 4.0f;
				}
				newNode = new AStarNode(x - 1, y, a.getFScore() + weight, distFromPlayer(x - 1, y), Direction.Left);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
			if ( x + 1 < GameTools.Map.size_x ) {
				weight = 1.0f;
				if (GameTools.Map.map_unit_occupy[x + 1, y]) {
					weight = 4.0f;
				}
				newNode = new AStarNode(x + 1, y, a.getFScore() + weight, distFromPlayer(x + 1, y), Direction.Right);
				newNode.Prev = a;
				listOfNeighbours.Add (newNode);
			}
		
			weight = 1.0f;
			if (y - 1 >= 0 && GameTools.Map.map_unit_occupy[x, y - 1]) {
				weight = 4.0f;
			}
			newNode = new AStarNode(x, y - 1, a.getFScore() + weight, distFromPlayer(x, y - 1), Direction.Down);
			newNode.Prev = a;
			listOfNeighbours.Add (newNode);

			weight = 1.0f;
			if (y + 1 < GameTools.Map.size_z && GameTools.Map.map_unit_occupy[x, y + 1]) {
				weight = 4.0f;
			}
			newNode = new AStarNode(x, y + 1, a.getFScore() + weight, distFromPlayer(x, y + 1), Direction.Up);
			newNode.Prev = a;
			listOfNeighbours.Add (newNode);
		}
		return listOfNeighbours;
	}
	//if (x - 1 >= 0 && GameTools.Map.map_unit_occupy[x - 1, y] != true /*GameTools.Map.map.Map_data[x-1, y] != 0*/) {
	//if (x + 1 < GameTools.Map.size_x && GameTools.Map.map_unit_occupy[x + 1, y] != true /*x + 1 < GameTools.Map.size_x && GameTools.Map.map.Map_data[x+1, y] != 0*/) {
	//if (y - 1 >= 0 && GameTools.Map.map_unit_occupy[x, y - 1] != true /*y - 1 >= 0 && GameTools.Map.map.Map_data[x, y-1] != 0*/) {
	//if (y + 1 < GameTools.Map.size_z && GameTools.Map.map_unit_occupy[x, y + 1] != true/*y + 1 < GameTools.Map.size_z && GameTools.Map.map.Map_data[x, y+1] != 0*/) {
	
	Stack<Direction> findPath(int destinationX, int destinationY) {
		IComparer comparer = new AStarComparer();
		Heap<AStarNode> openSet = new Heap<AStarNode>(comparer);
		HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
		bool found = false;
		int debugCount = 0;
		
		int x = Map_position_x, y = Map_position_y;
		AStarNode nodeCurrentPosition = new AStarNode(x, y, 0.0f, distFromPlayer(x, y), Direction.None);
		AStarNode nodeDestination = new AStarNode(destinationX, destinationY);
		
		openSet.insert(nodeCurrentPosition);
		
		while (openSet.length() > 0) {
			debugCount++;
			if (debugCount > 1000) {
				Debug.LogError("findPath in infinite loop");
				break;
			}
			nodeCurrentPosition = openSet.extract();
			closedSet.Add(nodeCurrentPosition);
			
			//use hash value instead of equality because of astarnode implementation 		-- CHANGE THIS
			if (closedSet.Contains(nodeDestination)) {
				//found the optimal path
				//Debug.LogError("Optimal path found");
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
		if (openSet.length() == 0) {
			Debug.Log("Enemy no valid move");
		}
		Stack<Direction> path = new Stack<Direction>();
		if (!found) {
			Debug.Log("Pathfinding hasn't found path to player");
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

	public void animation_tick() {
		if (Health <= 0) {
			//IsDead = true;
			FinishedAnimation = true;
			GameTools.TM.signalDeath(this);
			return;
		}
		if (current_target == Direction.None) {
			if (list_directions.Count > 0 && list_directions[0] != Direction.None) {
				current_target = list_directions[0];
				list_directions.RemoveAt(0);
				remainingDistance = 1.0f;
				FinishedAnimation = false;
			} else {
				//there's no current target and there's no directions in the directions list
				FinishedAnimation = true;
				return;
			}
		}
			
		switch (current_target) {
			case Direction.Up:
				unit.transform.Translate(0, 0, MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Down:
				unit.transform.Translate(0, 0, -MoveSpeed * Time.deltaTime, null);
				break;
			case Direction.Left:
				unit.transform.Translate(-MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			case Direction.Right:
				unit.transform.Translate(MoveSpeed * Time.deltaTime, 0, 0, null);
				break;
			default:
				Debug.Log ("Defaulted");
				break;
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance < 0) {
			//We've arrived at our destination, but overshot a little bit
			//correct overshooting
            Vector3 temp = unit.transform.position;
            unit.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));

			current_target = Direction.None;
		}

	}

	public void death_tick() {
		//display death animation (if any)
		FinishedAnimation = true;			//temp no animation, just return immediately
		Debug.Log ("Death Tick");
	}

	public void addMove(Direction d) {
		list_directions.Add(d);
	}


}
