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
		Map_position_x = Random.Range(-10, 10);
		Map_position_y = Random.Range(-10, 10);
        unit.transform.position = new Vector3(Map_position_x, 0, Map_position_y);
		Health = 10f;
		MoveSpeed = 10.0f;
	}

	public void determineNextMove() {
		Stack<Direction> stackOfDirections = findPath(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y);
		list_directions = new List<Direction>();
		if (stackOfDirections.Count == 0) {
			Debug.LogError("StackOfDirections is 0");
		}
		Direction d = stackOfDirections.Pop();
		if (d == Direction.None) {
			Debug.Log ("Got the none direction");
			if (stackOfDirections.Count == 0) {
				Debug.LogError ("That's it?");
				return;
			}
			d = stackOfDirections.Pop ();
			if (d == Direction.None) {
				Debug.LogError("Something's wrong");
			} else {
				Debug.Log ("It works i think");
				list_directions.Add (d);
			}
		}
	}
	
	
	float distFromPlayer(float x, float y) {
		float ans = Mathf.Sqrt((GameTools.Player.Map_position_x - x) * (GameTools.Player.Map_position_x - x) + 
		                       (GameTools.Player.Map_position_y - y) * (GameTools.Player.Map_position_y - y));
		if (ans < 0) {
			Debug.LogError("Negative distance");
		}
		return ans;
	}

	List<AStarNode> findNeighbours(AStarNode a) {
		List<AStarNode> listOfNeighbours = new List<AStarNode>();
		float weight = 1.0f;	//let weight be 1 for now
		int x = a.CoOrds[0], y = a.CoOrds[1];
		AStarNode newNode;
		//if (x - 1 >= 0/* && GameTools.Map.map.Map_data[x-1, y] != 0*/) {
			newNode = new AStarNode(x - 1, y, a.getFScore() + weight, distFromPlayer(x - 1, y), Direction.Left);
			newNode.Prev = a;
			listOfNeighbours.Add (newNode);
		//}
		//if (x + 1 < GameTools.Map.size_x/* && GameTools.Map.map.Map_data[x+1, y] != 0*/) {
			newNode = new AStarNode(x + 1, y, a.getFScore() + weight, distFromPlayer(x + 1, y), Direction.Right);
			newNode.Prev = a;
			listOfNeighbours.Add (newNode);
		//}
		//if (y - 1 >= 0/* && GameTools.Map.map.Map_data[x, y-1] != 0*/) {
			newNode = new AStarNode(x, y - 1, a.getFScore() + weight, distFromPlayer(x, y - 1), Direction.Down);
			newNode.Prev = a;
			listOfNeighbours.Add (newNode);
		//}
		//if (y + 1 < GameTools.Map.size_z/* && GameTools.Map.map.Map_data[x, y+1] != 0*/) {
			newNode = new AStarNode(x, y + 1, a.getFScore() + weight, distFromPlayer(x, y + 1), Direction.Up);
			newNode.Prev = a;
			listOfNeighbours.Add (newNode);
		//}
		return listOfNeighbours;
	}
	
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
			if (debugCount > 500) {
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
			Debug.LogError("Open set is empty?");
		}
		Stack<Direction> path = new Stack<Direction>();
		if (!found) {
			Debug.LogError("Pathfinding hasn't found path to player");
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
			if (list_directions.Count > 0) {
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
		}
		remainingDistance -= MoveSpeed * Time.deltaTime;
		if (remainingDistance < 0) {
			//We've arrived at our destination, but overshot a little bit
			//correct overshooting
            Vector3 temp = unit.transform.position;
            unit.transform.position = new Vector3(Mathf.Round(temp.x), temp.y, Mathf.Round(temp.z));
			switch (current_target) {
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
