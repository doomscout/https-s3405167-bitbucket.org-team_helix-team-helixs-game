using UnityEngine;
using System.Collections;

public class GraphNode {

	public int x {get;set;}
	public int y {get;set;}
	public GraphNode Prev {get;set;}

	public GraphNode(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals (object obj) {
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}
		GraphNode a = obj as GraphNode;
		return a.x == x && a.y == y;
	}

	public override int GetHashCode () {
		return (x + " " + y).GetHashCode();
	}

}
