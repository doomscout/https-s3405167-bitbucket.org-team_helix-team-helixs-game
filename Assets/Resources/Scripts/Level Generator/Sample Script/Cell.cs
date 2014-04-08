/*Scripter: Ke Yi Ren
 *Reference:
 *
 *
 */
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*Structure of Tile*/

public class Cell{

	public enum SideType
	{
		Empty,
		Edge,
		Door
	}

	public enum DirectionType
	{
		North,
		South,
		East,
		West
	}

	public enum TileType
	{
		Void = 0,
		Ground = 1,
		Corridor = 2,
		Room = 3,
		DoorNS 	= 4,
		DoorEW = 5,

	}

	private SideType northSide = SideType.Edge;
	private SideType southSide = SideType.Edge;
	private SideType eastSide = SideType.Edge;
	private SideType westSide = SideType.Edge;

	private bool visited;
	private bool isCorridor;

	public SideType NorthSide
	{
		get {return northSide; }
		set {northSide = value;}
	}

	public SideType SouthSide
	{
		get {return southSide; }
		set {southSide = value;}
	}

	public SideType EastSide
	{
		get{return eastSide;}
		set {eastSide = value;}
	}

	public SideType WestSide
	{
		get{return westSide;}
		set{westSide = value;}
	}

	public bool Visited
	{
		get{ return visited;}
		set{ visited = value;}
	}
	public bool IsDeadEnd
	{
		get { return EdgeCount == 3;}
	}

	public int EdgeCount
	{
		get
		{
			int edgeCount = 0;
			if (northSide == SideType.Edge) edgeCount++;
			if (southSide == SideType.Edge) edgeCount++;
			if (westSide == SideType.Edge) edgeCount++;
			if (eastSide == SideType.Edge) edgeCount++;
			return edgeCount;
		}
	}

	public DirectionType CaluacteDeadEndCorridorDirection()
	{
		if(!IsDeadEnd) throw new Exception();
		if(northSide == SideType.Empty) return DirectionType.North;
		if(southSide == SideType.Empty) return DirectionType.South;
		if(westSide == SideType.Empty) return DirectionType.West;
		if(eastSide == SideType.Empty) return DirectionType.East;

		throw new Exception();
	}
}
