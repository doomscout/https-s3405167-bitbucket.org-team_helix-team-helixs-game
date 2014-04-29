using UnityEngine;
using System.Collections;

public class Shape {

	private readonly int[,] coneShape = new int[5,4]{	{0, 0, 0, 1},
														{0, 0, 1, 1},
														{1, 1, 1, 1},
														{0, 0, 1, 1},
														{0, 0, 0, 1}
													};
	public string spellShape;
	public float shapeModifier;

	public Shape() {
		setRandomShape();
	}

	public Shape(string shape) : this() {
		spellShape = shape;
	}

	public void setRandomShape() {
		int set = (int)Random.Range(1,5); 
		
		if(set==1) {
			spellShape = "line";
			shapeModifier = 0.8f;
		}
		else if(set==2) {
			spellShape = "circle";
			shapeModifier = 0.3f;
		}
		else if(set==3) {
			spellShape = "single";
			shapeModifier = 0.9f;
		}
		else if(set==4) {
			spellShape = "cone";
			shapeModifier = 0.5f;
		}
		else if(set==5) {
			spellShape = "floor";
			shapeModifier = 0.2f;
		}

	}

	public string getShape() {
		return spellShape;
	}

	public int[,] toCoords(int[] centre, int[] mouse) {
		int[,] coordArray;
		int c_x = centre[0];
		int c_y = centre[1];
		int m_x = mouse[0];
		int m_y = mouse[1];

		if(spellShape=="line") {
			//calculate direction, extrapolate 4 tiles
			coordArray = new int[7,2];
			int x = m_x - c_x;
			int y = m_y - c_y;

			if (x == 0 && y == 0) {
				//Debug.LogError ("No direction");
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

			for(int i = 0; i < 7; i++) {
				coordArray[i, 0] = c_x + (x * (i+1));
				coordArray[i, 1] = c_y + (y * (i+1));
			}

			return coordArray;
		}

		else if(spellShape=="circle") {
			//circle centred around player coordinates. Hard coded.
			coordArray = new int[5,2];

			//coordArray[0,0] = m_x-2;
			//coordArray[0,1] = m_y;

			coordArray[0,0] = m_x-1;
			coordArray[0,1] = m_y;

			//coordArray[1,0] = m_x-1;
			//coordArray[1,1] = m_y+1;

			//coordArray[2,0] = m_x-1;
			//coordArray[2,1] = m_y-1;

			coordArray[1,0] = m_x;
			coordArray[1,1] = m_y-1;

			//coordArray[5,0] = m_x;
			//coordArray[5,1] = m_y-2;
			
			coordArray[2,0] = m_x;
			coordArray[2,1] = m_y+1;
			
			//coordArray[7,0] = m_x;
			//coordArray[7,1] = m_y+2;
			
			coordArray[3,0] = m_x+1;
			coordArray[3,1] = m_y;
			
			//coordArray[6,0] = m_x+1;
			//coordArray[6,1] = m_y-1;

			//coordArray[7,0] = m_x+1;
			//coordArray[7,1] = m_y+1;
			
			//coordArray[11,0] = m_x+2;
			//coordArray[11,1] = m_y;

			coordArray[4,0] = m_x;
			coordArray[4,1] = m_y;
			
			return coordArray;
		}
		else if(spellShape=="single") {
			//centered around mouse coordinates. No limit on range.
			coordArray = new int[1,2];
			/* this code imposes a limit on range
			int x = c_x - m_x;
			int y = c_y - m_y;
			if (Mathf.Abs (x) + Mathf.Abs(y) > 5) {
				return new int[0,0];
			}
			*/

			coordArray[0,0] = m_x;
			coordArray[0,1] = m_y;

			return coordArray;
		}
		else if(spellShape=="cone") {
			//calculates a line, then the surrounding cone tiles

			return shapeToWorldSpace(centre, mouse, coneShape);

		}
		else if(spellShape=="floor")	{
			//use gametools.map to find the coordinates of all enemy units;

			//filler code
			coordArray = new int[GameTools.GI.list_live_units.Count, 2];
			for (int i = 0; i < GameTools.GI.list_live_units.Count; i++) {
				coordArray[i,0] = GameTools.GI.list_live_units[i].Map_position_x;
				coordArray[i,1] = GameTools.GI.list_live_units[i].Map_position_y;
			}

			
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