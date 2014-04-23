using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode {

	public float FScore {get;set;}		// G + H score
	public float GScore {get;set;}		// Travelled so far
	public float HScore {get;set;}		// Heuristic score
	public Direction d {get;set;}
	public int[] CoOrds {get;set;}
	public AStarNode Next{get;set;}

	public AStarNode(int x, int y) {
		FScore = 0.0f;
		GScore = 0.0f;
		HScore = 0.0f;
		d = Direction.None;
		CoOrds = new int[2];
		CoOrds[0] = x;
		CoOrds[1] = y;
		Next = null;
	}

	public AStarNode(int[,] CoOrds) : this(){
		this.CoOrds = CoOrds;
	}
	
	public static bool operator <(AStarNode a, AStarNode b) {
		return a.FScore < b.FScore;
	}
	
	public static bool operator >(AStarNode a, AStarNode b) {
		return a.FScore > b.FScore;
	}
	
	public static bool operator ==(AStarNode a, AStarNode b) {
		return a.FScore == b.FScore;
	}
	
	public static bool operator !=(AStarNode a, AStarNode b) {
		return a.FScore != b.FScore;
	}
	
	public override string ToString () {
		return "FScore: " + FScore + "GScore: " + GScore + "HScore: " + HScore + "CoOrds: " + CoOrds[0] + "," + CoOrds[1];
	}
	
	public override bool Equals (object obj) {
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}
		AStarNode a = obj as AStarNode;
		return this.ToString() == a.ToString();
	}

	public override int GetHashCode () {
		return this.ToString().GetHashCode();
	}

}
