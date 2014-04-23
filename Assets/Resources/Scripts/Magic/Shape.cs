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