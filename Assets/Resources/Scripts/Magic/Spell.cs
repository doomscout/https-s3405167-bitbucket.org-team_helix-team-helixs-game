using UnityEngine;
using System.Collections;

public class Spell {
	public Shape Shape {get; private set;}
	public Colour SpellColour {get; private set;}
	public float Power {get; private set;}
    public int CastRange {get; set;}
	public Effect SpellEffect {get; private set;}
	public float SpellRating {get; private set;}

    private int[] loadedOrigin;
	private int[] loadedDestination;
	private bool hasLoadInfo = false;

	public Spell () {
		SpellColour = ColourManager.getRandomColour();
		Shape = new Shape();
		Power = Random.Range(1, 20);
		SpellEffect = new StatusEffect (	Random.Range(1, 10), /* tick count */
		                   					Random.Range(1, 5),  /* power */
		                                	(StatusType)Random.Range(0, System.Enum.GetNames(typeof(StatusType)).Length));
		CastRange = Shape.CastRange;
		SpellRating = calculateRating();
	}

	public Spell (ShapeType shape) : this() {
		Shape = new Shape(shape);
		CastRange = Shape.CastRange;
		SpellRating = calculateRating();
	}

	public Spell (Colour c) : this() {
		SpellColour = c;
	}

	public Spell (ShapeType shape, Colour c) : this(shape) {
		SpellColour = c;
	}

	private float calculateRating() {
		/* Variables:
		 * power
		 * cast range
		 * affected tiles
		 * isPlayerCentred
		 * status - tickCount
		 * status - power
		 * status - effect
		 */

		/* Direct damage weights */
		float powerWeight = 1.0f;
		float castRangeWeight = 0.5f;
		float affectedTilesWeight = 2.0f;
		/* status weights */
		float tickCountWeight = 1.0f;
		float statusPowerWeight = 1.5f;
		/* additional modifiers */
		float powerModifier = 1.0f;
		float statusEffectModifier = 1.50f;

		float rating = 0;
		/* Direct damage */
		rating += 	((Power * powerWeight) + 
		            (CastRange * CastRange * castRangeWeight) + 
		            (Shape.numberOfOnes * affectedTilesWeight)) * powerModifier;

		/* status */
		rating += 	((SpellEffect.TickCount * tickCountWeight) *
		            (SpellEffect.Power * statusPowerWeight)) * statusEffectModifier;

		return rating;
	}

	public void loadInfo(int[] origin, int[] position) {
		loadedOrigin = origin;
		loadedDestination = position;
		hasLoadInfo = true;
	}

	public void cast(int[] origin, int[] position) {
		int[,] coordinates = Shape.toCoords(origin, position);
		bool isTrap = true;
		for (int i = 0; i < coordinates.GetLength(0); i++) {

			if (MapTools.IsOutOfBounds(coordinates[i,0], coordinates[i,1])) {
				continue;
			}
			isTrap = true;

			if (GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]] != null) {
				GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]].GetHitByMagic(this);
				isTrap = false;
			}
			if (GameTools.Map.BonusTileData[coordinates[i,0], coordinates[i,1]] != null) {
				GameTools.Map.BonusTileData[coordinates[i,0], coordinates[i,1]].GetHitByMagic(this);
				isTrap = false;
			}
			if (coordinates[i,0] == GameTools.Player.Map_position_x && coordinates[i,1] == GameTools.Player.Map_position_y) {
				GameTools.Player.GetHitByMagic(this);
				isTrap = false;
			}
			if (GameTools.Base.IsWithinBase(coordinates[i,0], coordinates[i,1])) {
				GameTools.Base.GetHitByMagic(this);
				isTrap = false;
			}
			if (isTrap) {
				new Trap(this, coordinates[i,0], coordinates[i,1]);
			}
		}			

	}

	public bool cast() {
		if (hasLoadInfo) {
			cast(loadedOrigin, loadedDestination);
			return true;
		}
		Debug.LogError("Tried casting without loading info, this is bug");
		return false;
	}

	public override string ToString () {
		return string.Format ("[Spell: Shape={0}, SpellColour={1}, Power={2}, CastRange={3}, SpellEffect={4}, EffectTick={5}SpellRating={6}]", Shape, SpellColour, Power, CastRange, SpellEffect.EffectName(), SpellEffect.TickCount, SpellRating);
	}
}
