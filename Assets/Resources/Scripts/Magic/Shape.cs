using UnityEngine;
using System.Collections;

public enum ShapeType {Line, Circle, Single, Cone, PCG }

public class Shape {

	private const int P = -1;	//Player symbol
	private const int M = -2;	//Mouse symbol
	private readonly int[,] singleShape = new int[1,1]{	{M} 
														};
	private readonly int[,] circleShape = new int[5,5]{	{0, 0, 0, 0, 0},
														{0, 0, 1, 0, 0},
														{0, 1, M, 1, 0},
														{0, 0, 1, 0, 0},
														{0, 0, 0, 0, 0}
													  };
	private readonly int[,] lineShape = new int[1,7]{	{P, 1, 1, 1, 1, 1, 1} 
													};

	private readonly int[,] coneShape = new int[5,5]{	{0, 0, 0, 0, 1},
														{0, 0, 0, 1, 1},
														{P, 1, 1, 1, 1},
														{0, 0, 0, 1, 1},
														{0, 0, 0, 0, 1}
													};
	private int[,] PCGShape;


	public ShapeType SpellShape {get; private set;}
	public int CastRange {get; private set;}
	private bool isPlayerCentered;

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
			CastRange = 5;
		} else {
			CastRange = 0;
		}
		if (SpellShape == ShapeType.PCG) {
			PCGShape = generatePCGShapeMirror();
		}
	}

	private void setRandomShape() {
		int shapeType = Random.Range(0, 5);
		SpellShape = (ShapeType)shapeType;

	}

	public int[,] toCoords(int[] centre, int[] mouse) {
		int[,] coordArray;
		int c_x = centre[0];
		int c_y = centre[1];
		int m_x = mouse[0];
		int m_y = mouse[1];

		switch (SpellShape) {
		case ShapeType.Single:
			return shapeToWorldSpace(centre, mouse, singleShape);
		case ShapeType.Circle:
			return shapeToWorldSpace(centre, mouse, circleShape);
		case ShapeType.Line:
			return shapeToWorldSpace(centre, mouse, lineShape);
		case ShapeType.Cone:
			return shapeToWorldSpace(centre, mouse, coneShape);
		case ShapeType.PCG:
			return shapeToWorldSpace(centre, mouse, PCGShape);
			break;
		}
		Debug.LogError("null shape type");
		return null;
	}

	//move one square at a time
	private int[,] generatePCGShapeMirror() {
		int[,] test = new int[5,5];
		int numtiles = 10;
		int core = Random.Range(1, numtiles/2);

		if (core % 2 == 1) {
			core++;
		}
		numtiles -= core;
		numtiles /= 2;

		test[2,0] = P;
		int r = Random.Range(0, core+1);
		for (int i = 1; i < core+1; i++) {
			test[2,i] = 1;
		}
		test[1,r] = 1;
		numtiles--;
		int debugCount = 0;
		while (numtiles > 0) {
			int row = Random.Range (0, 2);
			if (Random.Range(0, 2) == 0) {
				//left ro right
				for (int i = 0; i < test.GetLength(1); i++) {
					if (test[row,i] == 0) {
						if (i == r || (i + 1 < test.GetLength(1) && test[row,i+1] == 1)) {
							test[row, i] = 1;
							numtiles--;
						}
					}
				}
			} else {
				//right to left
				Debug.Log ("asdqwdqwdwd");
				for (int i = test.GetLength(1) - 1; i >= 0; i--) {
					if (test[row,i] == 0) {
						if (i == r || ( i - 1 >= 0 && test[row,i-1] == 1)) {
							test[row, i] = 1;
							numtiles--;
						}
					}
				}
			}
			if (debugCount++ > 1000) {
				Debug.LogError ("Overflow");
				return null;
			}
		}
		for (int i = 0; i < test.GetLength(0); i++) {
			for (int j = 0; j < test.GetLength(1); j++) {
				if (test[i,j] == 1) {
					test[test.GetLength(0)- 1 - i, j] = 1;
				}
			}
		}
		return test;
	}

	//move one square at a time
	private int[,] generatePCGShapeRandom() {
		int[,] test = new int[5,10];
		int numTiles = 15;
		int ranX = 1;
		int ranY = 2;
		bool validMove;

		test[2,0] = P;

		while (numTiles >= 0) {
			validMove = false;
			if (test[ranY,ranX] == 0) {
				test[ranY,ranX] = 1;
				numTiles--;
			}
			do {
				Direction move = (Direction)Random.Range(1, 4);
				int oldX = ranX;
				int oldY = ranY;
				validMove = true;
				switch (move) {
				case Direction.Down:
					ranY--;
					break;
				case Direction.Up:
					ranY++;
					break;
				case Direction.Left:
					ranX--;
					break;
				case Direction.Right:
					ranX++;
					break;
				}
				if (ranY < 0 || ranY > 4 ||
				    ranX < 1 || ranX > 9) {
					ranX = oldX;
					ranY = oldY;
					validMove = false;
				}
			} while (!validMove);
		}
		Debug.Log("Made shape");
		return test;
	}


	//this method assumes that if isplayercentered then intShape is pointing to the right
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
				} else if (intShape[i,j] == P) {
					isPlayerCentered = true;
				} else if (intShape[i,j] == M) {
					numberOfOnes++;
					isPlayerCentered = false;
				}
			}
		}

		/* this code imposes a limit on range */
		if (!isPlayerCentered && Mathf.Abs (x) + Mathf.Abs(y) > CastRange) {
			return new int[0,0];
		}

		//Since we know how many ones, we know how many tiles this spell affects
		coordArray = new int[numberOfOnes,2];
		
		//Put the shape in a coordinate system centered around the origin
		int x_offset = !isPlayerCentered? (intShape.GetLength(1)/2):0;
		for (int i = 0; i < intShape.GetLength(0); i++) {
			for (int j = 0; j < intShape.GetLength(1); j++) {
				if (intShape[i, j] == 1) {	
					coordArray[count,0] = j - x_offset;
					coordArray[count,1] = i - (intShape.GetLength(0)/2); /* Center around origin */
					count++;
				}
			}
		}
		if (isPlayerCentered) {
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
		if (isPlayerCentered) {
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


/*
	public void setRandomShape() {
		int set = (int)Random.Range(1,5); 
		
		if(set==1) {
			spellShape = "line";
			shapeModifier = 0.8f;
			castRange = 0;
		}
		else if(set==2) {
			spellShape = "circle";
			shapeModifier = 0.3f;
			castRange = 5;
		}
		else if(set==3) {
			spellShape = "single";
			shapeModifier = 0.9f;
			castRange = 5;
		}
		else if(set==4) {
			spellShape = "cone";
			shapeModifier = 0.5f;
			castRange = 0;
		}
		else if(set==5) {
			spellShape = "floor";
			shapeModifier = 0.2f;
			castRange = 0;
		}

	}
	*/