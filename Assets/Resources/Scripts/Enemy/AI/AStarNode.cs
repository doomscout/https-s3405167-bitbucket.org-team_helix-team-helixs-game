﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarNode {

	private float GScore;					//Travelled so far
	private float HScore;					//Heuristic
	public Direction d {get; private set;}
	public int[] CoOrds {get; private set;}
	public AStarNode Prev{get;set;}

	public AStarNode (int x, int y) {
		CoOrds = new int[2];
		CoOrds[0] = x;
		CoOrds[1] = y;
		GScore = 0.0f;
		HScore = 0.0f;
		d = Direction.None;
	}

	public AStarNode (int x, int y, float gscore, float hscore, Direction d) : this (x, y) {
		this.GScore = gscore;
		this.HScore = hscore;

		this.d = d;
		Prev = null;
	}

	public float getFScore() {
		return GScore + HScore;
	}
	/*
	public static bool operator <(AStarNode a, AStarNode b) {
		if (a == null || b == null) {
			return false;
		}
		return a.getFScore() < b.getFScore();
	}
	
	public static bool operator >(AStarNode a, AStarNode b) {
		if (a == null || b == null) {
			return false;
		}
		return a.getFScore() > b.getFScore();
	}
	
	public static bool operator ==(AStarNode a, AStarNode b) {
		if (a is null || b is null) {
			return false;
		}
		return a.getFScore() == b.getFScore();
	}
	
	public static bool operator !=(AStarNode a, AStarNode b) {
		if (a == null || b == null) {
			return false;
		}
		return a.getFScore() != b.getFScore();
	}
	*/
	public override string ToString () {
		return "CoOrds: " + CoOrds[0] + "," + CoOrds[1];
	}
	
	public override bool Equals (object obj) {
		if (obj == null || GetType() != obj.GetType()) {
			return false;
		}
		AStarNode a = obj as AStarNode;
		return this.ToString() == a.ToString();
	}

	public override int GetHashCode () {
		//return ((CoOrds[0]<<8) | CoOrds[1]).GetHashCode();
		return this.ToString().GetHashCode();
	}

}
