using UnityEngine;
using System.Collections;

public class Colour : MonoBehaviour {
	private string weakness;
	private string strength;
	private string colour;

	public string getWeakness(){
		return weakness;
	}

	public void setWeakness(string colour){
		weakness = colour;
	}

	public string getStrength(){
		return strength;
	}

	public void setStrength(string colour){
		strength = colour;
	}

	public void setColour(){
		int set = (int)Random.Range(1,6);

		if(set==1)
			colour = "red";
		else if(set==2)
			colour = "blue";
		else if(set==3)
			colour = "green";
		else if(set==4)
			colour = "yellow";
		else if(set==5)
			colour = "purple";
		else if(set==6)
			colour = "pink";
	}

	public string getColour(){
		return colour;
	}
}

public class Red : Colour {
	private Colour colour;

	public void red(Colour colour)
	{
		this.colour = colour;

		colour.setWeakness("blue");
		colour.setStrength("pink");
	}
}

public class Blue : Colour {
	private Colour colour;
	
	public void blue(Colour colour)
	{
		this.colour = colour;

		colour.setWeakness("green");
		colour.setStrength("red");
	}
}

public class Green : Colour {
	private Colour colour;
	
	public void green(Colour colour)
	{
		this.colour = colour;
		
		colour.setWeakness("yellow");
		colour.setStrength("blue");
	}
}

public class Yellow : Colour {
	private Colour colour;
	
	public void yellow(Colour colour)
	{
		this.colour = colour;
		
		colour.setWeakness("purple");
		colour.setStrength("green");
	}
}

public class Purple : Colour {
	private Colour colour;
	
	public void purple(Colour colour)
	{
		this.colour = colour;
		
		colour.setWeakness("pink");
		colour.setStrength("yellow");
	}
}

public class Pink : Colour {
	private Colour colour;
	
	public void pink(Colour colour)
	{
		this.colour = colour;
		
		colour.setWeakness("red");
		colour.setStrength("purple");
	}
}