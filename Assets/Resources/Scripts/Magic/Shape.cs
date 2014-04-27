using UnityEngine;
using System.Collections;

public class Shape : TileMouseOver {
	private string spellShape;

	public void setShape() {
		int set = (int)Random.Range(1,6);
		
		if(set==1)
			spellShape = "line";
		else if(set==2)
			spellShape = "circle";
		else if(set==3)
			spellShape = "single";
		else if(set==4)
			spellShape = "cone";
		else if(set==5)
			spellShape = "floor";
	}

	public string getShape() {
		return spellShape;
	}

	public int[][] shapeSpell(int[] centre, int[] mouse, string shape) {
		int[][] coordArray;
		int c_x = centre[0];
		int c_y = centre[1];
		int m_x = mouse[0];
		int m_y = mouse[1];

		if(shape=="line") {
			//calculate direction, extrapolate 4 tiles
			coordArray = new int[4][];
			int x = m_x - c_x;
			int y = m_y - c_y;
			int dir = x/Mathf.Abs(x);

			for(int i = 0; i < 4; i++) {
				coordArray[i][0] = c_x + ((i+1)*dir);
				coordArray[i][1] = Mathf.RoundToInt((y/x) * coordArray[i][0]);
			}

			return coordArray;
			//hard coded system.
			/*coordArray = new int[5][];

			coordArray[0][0] = x;
			coordArray[0][1] = y;

			coordArray[1][0] = x+1;
			coordArray[1][1] = y;

			coordArray[2][0] = x+2;
			coordArray[2][1] = y;

			coordArray[3][0] = x-1;
			coordArray[3][1] = y;

			coordArray[4][0] = x-2;
			coordArray[4][1] = y;

			return coordArray;*/
		}
		else if(shape=="circle") {
			//circle centred around player coordinates. Hard coded.
			coordArray = new int[12][];

			coordArray[0][0] = c_x-2;
			coordArray[0][1] = c_y;

			coordArray[1][0] = c_x-1;
			coordArray[1][1] = c_y;

			coordArray[2][0] = c_x-1;
			coordArray[2][1] = c_y+1;

			coordArray[3][0] = c_x-1;
			coordArray[3][1] = c_y-1;

			coordArray[4][0] = c_x;
			coordArray[4][1] = c_y-1;

			coordArray[5][0] = c_x;
			coordArray[5][1] = c_y-2;
			
			coordArray[6][0] = c_x;
			coordArray[6][1] = c_y+1;
			
			coordArray[7][0] = c_x;
			coordArray[7][1] = c_y+2;
			
			coordArray[8][0] = c_x+1;
			coordArray[8][1] = c_y;
			
			coordArray[9][0] = c_x+1;
			coordArray[9][1] = c_y-1;

			coordArray[10][0] = c_x+1;
			coordArray[10][1] = c_y+1;
			
			coordArray[11][0] = c_x+2;
			coordArray[11][1] = c_y;
			
			return coordArray;
		}
		else if(shape=="single") {
			//centered around mouse coordinates. No limit on range.
			coordArray = new int[1][];
			coordArray[0][0] = m_x;
			coordArray[0][1] = m_y;

			return coordArray;
		}
		else if(shape=="cone") {
			//calculates a line, then the surrounding cone tiles
			coordArray = new int[9][];

			int x = m_x - c_x;
			int y = m_y - c_y;
			int dir;

			//only 4 cardinal directions
			if(Mathf.Abs(x) > Mathf.Abs(y)) {
				//line coord
				dir = x/Mathf.Abs(x);
				for(int i = 0; i < 3; i++) {
					coordArray[i][0] = c_x + ((i+1)*dir);
					coordArray[i][1] = c_y;
				}
				//cone coord
				coordArray[3][0] = coordArray[1][0];
				coordArray[3][0] = coordArray[1][1] + 1;

				coordArray[4][0] = coordArray[1][0];
				coordArray[4][0] = coordArray[1][1] - 1;

				coordArray[5][0] = coordArray[2][0];
				coordArray[5][0] = coordArray[2][1] + 1;

				coordArray[6][0] = coordArray[2][0];
				coordArray[6][0] = coordArray[2][1] + 2;

				coordArray[7][0] = coordArray[2][0];
				coordArray[7][0] = coordArray[2][1] - 1;

				coordArray[8][0] = coordArray[2][0];
				coordArray[8][0] = coordArray[2][1] - 2;
			}
			else {
				//line coord
				dir = y/Mathf.Abs(y);
				for(int i = 0; i < 3; i++) {
					coordArray[i][0] = c_x;
					coordArray[i][1] = c_y + ((i+1)*dir);
				}
				//cone coord
				coordArray[3][0] = coordArray[1][0] + 1;
				coordArray[3][0] = coordArray[1][1];
				
				coordArray[4][0] = coordArray[1][0] - 1;
				coordArray[4][0] = coordArray[1][1];
				
				coordArray[5][0] = coordArray[2][0] + 1;
				coordArray[5][0] = coordArray[2][1];
				
				coordArray[6][0] = coordArray[2][0] + 2;
				coordArray[6][0] = coordArray[2][1];
				
				coordArray[7][0] = coordArray[2][0] - 1;
				coordArray[7][0] = coordArray[2][1];
				
				coordArray[8][0] = coordArray[2][0] - 2;
				coordArray[8][0] = coordArray[2][1];
			}

			return coordArray;
		}
		else if(shape=="floor")	{
			//use gametools.map to find the coordinates of all enemy units;

			//filler code
			coordArray = new int[1][];
			coordArray[0][0] = m_x;
			coordArray[0][1] = m_y;
			
			return coordArray;
		}
		else return null;
	}
}

public class Line : Shape {

}

public class Circle : Shape {

}

public class Single : Shape {
	
}

public class Cone : Shape {
	
}

public class Floor : Shape {
	
}