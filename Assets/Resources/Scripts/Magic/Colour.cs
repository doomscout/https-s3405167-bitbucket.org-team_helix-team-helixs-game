using UnityEngine;
using System.Collections;

public enum Colour { None, Red, Blue, Green, Yellow, Purple, Pink };

public static class ColourManager {

	public static Colour getWeakness(Colour c) {
		switch(c) {
		case Colour.Red:
			return Colour.Blue;
		case Colour.Blue:
			return Colour.Green;
		case Colour.Green:
			return Colour.Yellow;
		case Colour.Yellow:
			return Colour.Purple;
		case Colour.Purple:
			return Colour.Pink;
		case Colour.Pink:
			return Colour.Red;
		default:
			return Colour.None;
		}
	}

	public static Colour getStrength(Colour c) {
		switch(c) {
		case Colour.Red:
			return Colour.Pink;
		case Colour.Blue:
			return Colour.Red;
		case Colour.Green:
			return Colour.Blue;
		case Colour.Yellow:
			return Colour.Green;
		case Colour.Purple:
			return Colour.Yellow;
		case Colour.Pink:
			return Colour.Purple;
		default:
			return Colour.None;
		}
	}
	
}