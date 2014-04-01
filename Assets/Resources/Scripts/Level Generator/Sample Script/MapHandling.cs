using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapHandler : MonoBehaviour {

	Random rand = new Random();



	public int MapWidth {get;set;}
	public int MapHeight {get; set;}
	public int PercentAreEdges{get; set;}


	public int[,] Map;	



	public void MakeTile()
	{
		for(int column = 0, row = 0; row <= MapHeight -1; row++)
		{
			for(column = 0; column <= MapWidth-1; column++)
			{
				Map[column, row] = PlaceEdgesLogic(column, row);
			}
		}

	}

	public int PlaceEdgesLogic(int x, int y)
	{
		int numEdges = GetAdjacenEdge(x, y, 1, 1);

		if(Map[x,y] == 1)
		{
			if(numEdges >= 4 )
			{
				return 1;
			}
			if(numEdges < 2)
			{
			return 0;
			}
		}
		else
		{
			if(numEdges >= 5)
			{
				return 1;
			}
		}
		return 0;
	}

	public int GetAdjacenEdge(int x, int y, int scopeX, int scopeY)
	{
		int startX = x - scopeX;
		int startY = y - scopeY;
		int endX = x + scopeX;
		int endY = y + scopeY;

		int iX = startX;
		int iY = startY;

		int edgesCounter = 0;
	

		for(iY = startY; iY <=endY; iY++)
		{
			for(iX = startX; iX <=endX; iX++)
			{
				if(!(iX==x && iY==y))
				{
					if(IsEdge(iX,iY))
					{
						edgesCounter +=1;
					}
				}
			}
		}
		return edgesCounter;
	}

	bool IsEdge(int x, int y)
	{

		if( IsOutOfBounds(x,y))
		{
			return true;
		}

		if( Map[x,y] == 1)
		{
			return true;
		}

		if(Map[x,y] ==0)
		{
			return false;
		}
		return false;
	}

	bool IsOutOfBounds(int x, int y)
	{
		if(x < 0 || y < 0)
		{
			return true;
		}
		else if ( x > MapWidth - 1 || y > MapHeight - 1)
		{
			return true;
		}
		return false;
	}

	public void PrintMap()
	{
	}

	public void RandomFillMap()
	{
		Map = new int[MapWidth, MapHeight];
		int mapMiddle = 0;
		for(int column = 0, row = 0; row < MapHeight; row++)
		{
			for(column = 0; column < MapWidth; column++)
			{
				if(column = 0)
				{
					Map[column, row] = 1;
				}
				else if (row == 0)
				{
					Map[column, row] = 1;
				}
				else if (column == MapWidth - 1)
				{
					Map[column, row] = 1;
				}
				else if (row == MapHeight -1)
				{
					Map[column, row] = 1;
				}

				else
				{
					mapMiddle = (MapHeight / 2);

					if(row == mapMiddle)
					{
						Map[column, row] = 0;
					}
					else
					{
						Map[column, row] = RandomPercent(PercentAreEdges);
					}
				}
			}
		}
	}

	int RandomPercent(int percent)
	{
		if(percent >= rand.Next(1,101))
		{
			return 1;
		}
		return 0;
	}
}
