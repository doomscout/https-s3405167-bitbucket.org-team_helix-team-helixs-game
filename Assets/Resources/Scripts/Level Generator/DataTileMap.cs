using UnityEngine;
using System.Collections.Generic;

public class DataTileMap {
	

	//We using  AtomataDungeonGenerator algorithm
	

	//MapWidth
	int Size_x {get;set;}
	//MapHeight
	int Size_y{get;set;}
	//Percent for fill EmptyTile
	int PercentAreEmpty{get;set;}
	public int[,] Map_data_passable;
	public Colour[,] Map_data;
	//public int[,] Store_data;
	
	/*
	 * 0 = null or white
	 * 1 = red
	 * 2 = blue
	 * 3 = Yellow
	 * 4 = Green
	 * 5 = Pink
	 * 6 = Purple
	 */
	
	public DataTileMap(int size_x, int size_y, int percentAreEmpty) {
		this.Size_x = size_x;
		this.Size_y = size_y;
		Map_data_passable = new int[size_x,size_y];
		Map_data = new Colour[size_x,size_y];
		this.PercentAreEmpty = percentAreEmpty;
		//GetTileAt(size_x, size_y);
		//MakeTile();
		RandomFillMap();
		MakeTile();
		GeneratorColorTile();
//		PrintDebug();
	}

	public int GetTileAt(int x, int y) {
			return (int)Map_data[x,y];
	}
			

	public void GeneratorColorTile()
	{
		for(int column = 0, row = 0; row <= Size_x -1; row++)
		{
			for(column = 0; column <= Size_y-1; column++)
			{
				//The range from Red, to the last colour in the enum
				int randomNumber = Random.Range(1, System.Enum.GetNames(typeof(Colour)).Length - 1);
				if(Map_data_passable[column, row] == 1)
					Map_data[column, row] = (Colour)randomNumber;
				//Debug.Log ("column = " + (column+1) + " row =  "+ (row+1) + " ColorNumber  "+ Map_data[column, row]);
			}
		}
	}


	public void MakeTile()
	{
		for(int column = 0, row = 0; row <= Size_x -1; row++)
		{
			for(column = 0; column <= Size_y-1; column++)
			{
				Map_data_passable[column, row] = PlaceEmptyLogic(column, row);
			}
		}
		
	}


	public int PlaceEmptyLogic(int x, int y)
	{
		int numEdges = GetAdjacenEmpty(x, y, 1, 1);
		
		if(Map_data_passable[x,y] == 0)
		{
			if(numEdges >= 4 )
			{
				return 0;
			}
			if(numEdges < 2)
			{
				return 1;
			}
		}
		else
		{
			if(numEdges >= 5)
			{
				return 0;
			}
		}
		return 1;
	}

	public int GetAdjacenEmpty(int x, int y, int scopeX, int scopeY)
	{
		int startX = x - scopeX;
		int startY = y - scopeY;
		int endX = x + scopeX;
		int endY = y + scopeY;
		
		int iX = startX;
		int iY = startY;
		
		int emptyCounter = 0;
		
		
		for(iY = startY; iY <=endY; iY++)
		{
			for(iX = startX; iX <=endX; iX++)
			{
				if(!(iX==x && iY==y))
				{
					if(IsEmpty(iX,iY))
					{
						emptyCounter +=1;
					}
				}
			}
		}
		return emptyCounter;
	}

	bool IsEmpty(int x, int y)
	{
		
		if( IsOutOfBounds(x,y))
		{
			return true;
		}
		
		if( Map_data_passable[x,y] == 0)
		{
			return true;
		}
		
		if( Map_data_passable[x,y] == 1 )
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
		else if ( x > Size_x - 1 || y > Size_y - 1)
		{
			return true;
		}
		return false;
	}

	
	public void RandomFillMap()
	{

		Map_data_passable = new int[Size_x, Size_y];
		int mapMiddle = 0;
		for(int column = 0, row = 0; row < Size_y; row++)
		{
			for(column = 0; column < Size_x; column++)
			{
				if(column == 0)
				{
					Map_data_passable[column, row] = 0;
				}
				else if (row == 0)
				{
					Map_data_passable[column, row] = 0;
				}
				else if (column == Size_x - 1)
				{
					Map_data_passable[column, row] = 0;
				}
				else if (row == Size_y -1)
				{
					Map_data_passable[column, row] = 0;
				}
				
				else
				{
					mapMiddle = (Size_y / 2);
					
					if(row == mapMiddle)
					{
						Map_data_passable[column, row] = 1;
					}
					else
					{
						Map_data_passable[column, row] = RandomPercent(PercentAreEmpty);
					}
				}
			}
		}
	}

	int RandomPercent(int percent)
	{
		if(PercentAreEmpty >= Random.Range(1,100)){
			return 1;
		}
		return 0;
	}
}

/*


	protected class DRoom {
		public int left;
		public int top;
		public int width;
		public int height;
		
		public bool isConnected=false;
		
		public int right {
			get {return left + width - 1;}
		}
		
		public int bottom {
			get { return top + height - 1; }
		}
		
		public int center_x {
			get { return left + width/2; }
		}
		
		public int center_y {
			get { return top + height/2; }
		}
		
		public bool CollidesWith(DRoom other) {
			if( left > other.right-1 )
				return false;
			
			if( top > other.bottom-1 )
				return false;
			
			if( right < other.left+1 )
				return false;
			
			if( bottom < other.top+1 )
				return false;
			
			return true;
		}
		
		
	}






	
		for(int x=0;x<size_x;x++) {
			for(int y=0;y<size_y;y++) {
				map_data[x,y] = 3;
			}
		}
		
		rooms = new List<DRoom>();
		
		int maxFails = 10;
		
		while(rooms.Count < 10) {
			int rsx = Random.Range(4,14);
			int rsy = Random.Range(4,10);
			
			r = new DRoom();
			r.left = Random.Range(0, size_x - rsx);
			r.top = Random.Range(0, size_y-rsy);
			r.width = rsx;
			r.height = rsy;
			
			if(!RoomCollides(r)) {			
				rooms.Add (r);
			}
			else {
				maxFails--;
				if(maxFails <=0)
					break;
			}
			
		}
		
		foreach(DRoom r2 in rooms) {
			MakeRoom(r2);
		}
		
		
		for(int i=0; i < rooms.Count; i++) {
			if(!rooms[i].isConnected) {
				int j = Random.Range(1, rooms.Count);
				MakeCorridor(rooms[i], rooms[(i + j) % rooms.Count ]);
			}
		}
		
		MakeWalls();
		
	}
	
	bool RoomCollides(DRoom r) {
		foreach(DRoom r2 in rooms) {
			if(r.CollidesWith(r2)) {
				return true;
			}
		}
		
		return false;
	}
	

	
	void MakeRoom(DRoom r) {
		
		for(int x=0; x < r.width; x++) {
			for(int y=0; y < r.height; y++){
				if(x==0 || x == r.width-1 || y==0 || y == r.height-1) {
					map_data[r.left+x,r.top+y] = 2;
				}
				else {
					map_data[r.left+x,r.top+y] = 1;
				}
			}
		}
		
	}
	
	void MakeCorridor(DRoom r1, DRoom r2) {
		int x = r1.center_x;
		int y = r1.center_y;
		
		while( x != r2.center_x) {
			map_data[x,y] = 1;
			
			x += x < r2.center_x ? 1 : -1;
		}
		
		while( y != r2.center_y ) {
			map_data[x,y] = 1;
			
			y += y < r2.center_y ? 1 : -1;
		}
		
		r1.isConnected = true;
		r2.isConnected = true;
		
	}
	
	void MakeWalls() {
		for(int x=0; x< size_x;x++) {
			for(int y=0; y< size_y;y++) {
				if(map_data[x,y]==3 && HasAdjacentFloor(x,y)) {
					map_data[x,y]=2;
				}
			}
		}
	}
	
	bool HasAdjacentFloor(int x, int y) {
		if( x > 0 && map_data[x-1,y] == 1 )
			return true;
		if( x < size_x-1 && map_data[x+1,y] == 1 )
			return true;
		if( y > 0 && map_data[x,y-1] == 1 )
			return true;
		if( y < size_y-1 && map_data[x,y+1] == 1 )
			return true;
		
		if( x > 0 && y > 0 && map_data[x-1,y-1] == 1 )
			return true;
		if( x < size_x-1 && y > 0 && map_data[x+1,y-1] == 1 )
			return true;
		
		if( x > 0 && y < size_y-1 && map_data[x-1,y+1] == 1 )
			return true;
		if( x < size_x-1 && y < size_y-1 && map_data[x+1,y+1] == 1 )
			return true;
		
		
		return false;
	}
*/
