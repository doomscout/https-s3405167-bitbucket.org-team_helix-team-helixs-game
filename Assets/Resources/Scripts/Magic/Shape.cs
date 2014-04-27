using UnityEngine;
using System.Collections;

public class Shape : TileMouseOver {

	private readonly int[,] coneShape = new int[3,3]{	{0, 0, 1},
														{1, 1, 1},
														{0, 0, 1}
													};
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

	public int[,] shapeSpell(int[] centre, int[] mouse, string shape) {
		int[,] coordArray;
		int c_x = centre[0];
		int c_y = centre[1];
		int m_x = mouse[0];
		int m_y = mouse[1];

		if(shape=="line") {
			//calculate direction, extrapolate 4 tiles
			coordArray = new int[4,2];
			int x = m_x - c_x;
			int y = m_y - c_y;

			if (x == 0 && y == 0) {
				Debug.LogError ("No direction");
				return new int[0,0];
			}

			//Divide them by the highest number so x and y range from [0,1]
			int div;
			if (x == 0) {
				div = Mathf.Abs(y);
			} else if (y == 0) {
				div = Mathf.Abs(x);
			} else {
				div = Mathf.Abs(x) > Mathf.Abs(y) ? Mathf.Abs(x) : Mathf.Abs(y);
			}

			x = x / div;
			y = y / div;

			for(int i = 0; i < 4; i++) {
				coordArray[i, 0] = c_x + (x * (i+1));
				coordArray[i, 1] = c_y + (y * (i+1));
			}

			return coordArray;
		}

		else if(shape=="circle") {
			//circle centred around player coordinates. Hard coded.
			coordArray = new int[12,2];

			coordArray[0,0] = c_x-2;
			coordArray[0,1] = c_y;

			coordArray[1,0] = c_x-1;
			coordArray[1,1] = c_y;

			coordArray[2,0] = c_x-1;
			coordArray[2,1] = c_y+1;

			coordArray[3,0] = c_x-1;
			coordArray[3,1] = c_y-1;

			coordArray[4,0] = c_x;
			coordArray[4,1] = c_y-1;

			coordArray[5,0] = c_x;
			coordArray[5,1] = c_y-2;
			
			coordArray[6,0] = c_x;
			coordArray[6,1] = c_y+1;
			
			coordArray[7,0] = c_x;
			coordArray[7,1] = c_y+2;
			
			coordArray[8,0] = c_x+1;
			coordArray[8,1] = c_y;
			
			coordArray[9,0] = c_x+1;
			coordArray[9,1] = c_y-1;

			coordArray[10,0] = c_x+1;
			coordArray[10,1] = c_y+1;
			
			coordArray[11,0] = c_x+2;
			coordArray[11,1] = c_y;
			
			return coordArray;
		}
		else if(shape=="single") {
			//centered around mouse coordinates. No limit on range.
			coordArray = new int[1,2];
			coordArray[0,0] = m_x;
			coordArray[0,1] = m_y;

			return coordArray;
		}
		else if(shape=="cone") {
			//calculates a line, then the surrounding cone tiles

			return shapeToWorldSpace(centre, mouse, coneShape);
			/*
			//only 4 cardinal directions
			if(Mathf.Abs(x) > Mathf.Abs(y)) {
				//line coord
				dir = x/Mathf.Abs(x);
				for(int i = 0; i < 3; i++) {
					coordArray[i,0] = c_x + ((i+1)*dir);
					coordArray[i,1] = c_y;
				}
				//cone coord
				coordArray[3,0] = coordArray[1,0];
				coordArray[3,0] = coordArray[1,1] + 1;

				coordArray[4,0] = coordArray[1,0];
				coordArray[4,0] = coordArray[1,1] - 1;

				coordArray[5,0] = coordArray[2,0];
				coordArray[5,0] = coordArray[2,1] + 1;

				coordArray[6,0] = coordArray[2,0];
				coordArray[6,0] = coordArray[2,1] + 2;

				coordArray[7,0] = coordArray[2,0];
				coordArray[7,0] = coordArray[2,1] - 1;

				coordArray[8,0] = coordArray[2,0];
				coordArray[8,0] = coordArray[2,1] - 2;
			}
			else {
				//line coord
				dir = y/Mathf.Abs(y);
				for(int i = 0; i < 3; i++) {
					coordArray[i,0] = c_x;
					coordArray[i,1] = c_y + ((i+1)*dir);
				}
				//cone coord
				coordArray[3,0] = coordArray[1,0] + 1;
				coordArray[3,0] = coordArray[1,1];
				
				coordArray[4,0] = coordArray[1,0] - 1;
				coordArray[4,0] = coordArray[1,1];
				
				coordArray[5,0] = coordArray[2,0] + 1;
				coordArray[5,0] = coordArray[2,1];
				
				coordArray[6,0] = coordArray[2,0] + 2;
				coordArray[6,0] = coordArray[2,1];
				
				coordArray[7,0] = coordArray[2,0] - 1;
				coordArray[7,0] = coordArray[2,1];
				
				coordArray[8,0] = coordArray[2,0] - 2;
				coordArray[8,0] = coordArray[2,1];
			}

			return coordArray;
			*/
		}
		else if(shape=="floor")	{
			//use gametools.map to find the coordinates of all enemy units;

			//filler code
			coordArray = new int[1,2];
			coordArray[0,0] = m_x;
			coordArray[0,1] = m_y;
			
			return coordArray;
		}

		else return null;
	}

	//this method assumes that the intShape is pointing to the right
	private int[,] shapeToWorldSpace (int[] centre, int[] mouse, int[,] intShape) {
		int c_x = centre[0];
		int c_y = centre[1];
		int x = mouse[0] - c_x;
		int y = mouse[1] - c_y;
		int numberOfOnes = 0;
		int count = 0;
		int[,] coordArray;

		if (x == 0 && y == 0) {
			//Debug.LogError ("No direction");
			return new int[0,0];
		}

		//Count the number of ones in the shape
		for (int i = 0; i < intShape.GetLength(0); i++) {
			for (int j = 0; j < intShape.GetLength(1); j++) {
				if (intShape[i,j] == 1) {
					numberOfOnes++;
				}
			}
		}
		//Since we know how many ones, we know how many tiles this spell affects
		coordArray = new int[numberOfOnes,2];
		
		//Put the shape in a coordinate system centered around the origin
		for (int i = 0; i < intShape.GetLength(0); i++) {
			for (int j = 0; j < intShape.GetLength(1); j++) {
				if (intShape[i, j] == 1) {	
					coordArray[count,0] = j + 1;
					coordArray[count,1] = i - (intShape.GetLength(0)/2); /* Center around origin */
					count++;
				}
			}
		}

		//Figure out which direction the spell is facing
		if (Mathf.Abs (x) > Mathf.Abs (y)) {
			//Flip horizontally
			if (x < 0) {
				//We're flipping the spell to face left
				coordArray = flipHorizontally(coordArray);
			} else {
				//The spell is already facing right, so do nothing
			}
		} else {
			//Flip vertically
			coordArray = flipDiagonally(coordArray);
			if (y < 0) {
				//Facing down
				coordArray = flipVertically(coordArray);
			} else {
				//Facing up, it is already like this with our flip Diagonally above
				//do nothing
			}
		}
		
		//Now move the spell from the origin to the player location
		for (int i = 0; i < coordArray.GetLength(0); i++) {
			coordArray[i,0] += c_x;
			coordArray[i,1] += c_y;
		}
		
		return coordArray;
	}

	//Flips the shape on the y = x line
	private int[,] flipDiagonally(int[,] old_xy) {
		int[,] new_xy = old_xy;
		for (int i = 0; i < old_xy.GetLength(0); i++) {
			int temp;
			temp = old_xy[i,1];
			new_xy[i,1] = old_xy[i,0];
			new_xy[i,0] = temp;
		}
		return new_xy;
	}

	//Flips on the x = 0 line
	private int[,] flipHorizontally(int[,] old_xy) {
		int[,] new_xy = old_xy;
		for (int i = 0; i < old_xy.GetLength(0); i++) {
			new_xy[i,0] = -old_xy[i,0];
			new_xy[i,1] = old_xy[i,1];
		}
		return new_xy;
	}

	//Flips on the y = 0 line
	private int[,] flipVertically(int[,] old_xy) {
		int[,] new_xy = old_xy;
		for (int i = 0; i < old_xy.GetLength(0); i++) {
			new_xy[i,0] = old_xy[i,0];
			new_xy[i,1] = -old_xy[i,1];
		}
		return new_xy;
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