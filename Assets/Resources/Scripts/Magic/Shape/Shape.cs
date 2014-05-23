using UnityEngine;
using System.Collections;

public enum ShapeType {Line, Circle, Single, Cone, PCG }

public class Shape {
	public ShapeType SpellShape {get; private set;}
	public int CastRange {get; private set;}
	public bool IsPlayerCentered;
	public int numberOfOnes;
	public int[,] shapeIntArray;

	public Shape() {
		setRandomShape();
		init ();
	}

	public Shape(ShapeType st) : this() {
		SpellShape = st;
		init ();
	}

	private void init() {
		if (SpellShape == ShapeType.Single || SpellShape == ShapeType.Circle) {
			CastRange = 50;
		} else {
			CastRange = 0;
		}
		switch (SpellShape) {
		case ShapeType.Single:
			shapeIntArray = ShapeInt.singleShape;
			break;
		case ShapeType.Circle:
			shapeIntArray = ShapeInt.circleShape;
			break;
		case ShapeType.Line:
			shapeIntArray = ShapeInt.lineShape;
			break;
		case ShapeType.Cone:
			shapeIntArray = ShapeInt.coneShape;
			break;
		case ShapeType.PCG:
			shapeIntArray = ShapeInt.GeneratePCGShapeMirror();
			break;
		}
	}

	private void setRandomShape() {
		int shapeType = Random.Range(0, 5);
		SpellShape = (ShapeType)shapeType;

	}

	public int[,] toCoords(int[] centre, int[] mouse) {
		return shapeToWorldSpace(centre, mouse, shapeIntArray);
	}

	//this method assumes that if isplayercentered then intShape is pointing to the right
	private int[,] shapeToWorldSpace (int[] centre, int[] mouse, int[,] intShape) {
		int c_x = centre[0];
		int c_y = centre[1];
		int x = mouse[0] - c_x;
		int y = mouse[1] - c_y;
		int count = 0;
		int[,] coordArray;

		numberOfOnes = 0;
		//Count the number of ones in the shape
		for (int i = 0; i < intShape.GetLength(0); i++) {
			for (int j = 0; j < intShape.GetLength(1); j++) {
				if (intShape[i,j] == 1) {
					numberOfOnes++;
				} else if (intShape[i,j] == ShapeInt.P) {
					IsPlayerCentered = true;
				} else if (intShape[i,j] == ShapeInt.M) {
					numberOfOnes++;
					IsPlayerCentered = false;
				}
			}
		}

		/* this code imposes a limit on range */
		if (!IsPlayerCentered && Mathf.Abs (x) + Mathf.Abs(y) > CastRange) {
			return new int[0,0];
		}

		if (IsPlayerCentered && x == 0 && y == 0) {
			//Debug.LogError ("No direction");
			return new int[0,0];
		}

		//Since we know how many ones, we know how many tiles this spell affects
		coordArray = new int[numberOfOnes,2];
		
		//Put the shape in a coordinate system centered around the origin
		int x_offset = !IsPlayerCentered? (intShape.GetLength(1)/2):0;
		for (int i = 0; i < intShape.GetLength(0); i++) {
			for (int j = 0; j < intShape.GetLength(1); j++) {
				if (intShape[i, j] == 1) {	
					coordArray[count,0] = j - x_offset;
					coordArray[count,1] = i - (intShape.GetLength(0)/2); /* Center around origin */
					count++;
				}
			}
		}
		if (IsPlayerCentered) {
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
		}
		
		//Now move the spell from the origin to the player location
		if (IsPlayerCentered) {
			for (int i = 0; i < coordArray.GetLength(0); i++) {
				coordArray[i,0] += c_x;
				coordArray[i,1] += c_y;
			}
		} else {
			for (int i = 0; i < coordArray.GetLength(0); i++) {
				coordArray[i,0] += mouse[0];
				coordArray[i,1] += mouse[1];
			}
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