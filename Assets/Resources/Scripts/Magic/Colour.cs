using UnityEngine;
using System.Collections;

public enum Colour { None, Red, Blue, Yellow, Green, Purple, Pink};

public static class ColourManager {

	public static readonly float WeaknessModifier = 0.5f;
	public static readonly float StrengthModifier = 1.5f;

	public static Colour getRandomColour() {
		return (Colour)Random.Range(1, System.Enum.GetNames(typeof(Colour)).Length - 1);
	}

	public static Color toColor(Colour c) {
		switch(c) {
		case Colour.Red:
			return Color.red;
		case Colour.Blue:
			return Color.blue;
		case Colour.Yellow:
			return Color.yellow;
		case Colour.Green:
			return Color.green;
		case Colour.Purple:
			return Color.magenta;
		case Colour.Pink:
			return new Color(255.0f/255.0f, 153.0f/255.0f, 255.0f/255.0f, 1.0f);
		default:
			return Color.white;
		}
	}

	public static Colour getWeakness(Colour c) {
		switch(c) {
		case Colour.Red:
			return Colour.Blue;
		case Colour.Blue:
			return Colour.Yellow;
		case Colour.Yellow:
			return Colour.Green;
		case Colour.Green:
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
		case Colour.Yellow:
			return Colour.Blue;
		case Colour.Green:
			return Colour.Yellow;
		case Colour.Purple:
			return Colour.Green;
		case Colour.Pink:
			return Colour.Purple;
		default:
			return Colour.None;
		}
	}
	
}