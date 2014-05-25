using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellIndicator : Cleanable{
	private GameObject[] pool;
	public bool IsShowingIndicator {get; private set;}
	private int refill = 0;
	private int max_pool_size;
	private CastRangeIndicator RangeIndicator;

	private List<int> TilesToAnimate;

	public SpellIndicator(int maxPoolSize) {
		max_pool_size = maxPoolSize;
		initSpellIndicator();
		CleanTools.GetInstance().SubscribeCleanable(this, true);
		TilesToAnimate = new List<int>();
	}

	public void CleanUp() {
		for (int i = 0; i < pool.Length; i++) {
			if (pool[i] != null) {
				Object.Destroy(pool[i]);
			}
		}
		RangeIndicator = null;
	}

	public void link(CastRangeIndicator cr) {
		RangeIndicator = cr;
	}

	public void showCastAnimation() {
		for (int i = 0; i < refill; i++) {
			if (TilesToAnimate.Contains(i)) {
				Indicator script = pool[i].transform.GetComponent<Indicator>();
				script.TriggerAnimation();
				TilesToAnimate.Remove(i);
				Debug.Log ("Animating");
			} else {
				GameObject.Destroy(pool[i]);
			}
		}
		refillPool();
		IsShowingIndicator = false;
	}

	public void showNoCastAnimation() {
		for (int i = 0; i < refill; i++) {
			Object.Destroy(pool[i]);
		}
		refillPool();
		IsShowingIndicator = false;
	}

	private void refillPool() {
		for (int i = 0; i < refill; i++) {
			pool[i] = Object.Instantiate(Resources.Load("Prefabs/Cube4", typeof(GameObject))) as GameObject;
			pool[i].transform.position = new Vector3(-100, i, 0);
		}
		refill = 0;
	}

	public void initSpellIndicator() {
		int temp = refill;
		IsShowingIndicator = false;
		if (pool == null) {
			pool = new GameObject[max_pool_size];
			temp = max_pool_size;
		} else {
			return;
		}
		for (int i = 0; i < temp; i++) {
			pool[i] = Object.Instantiate(Resources.Load("Prefabs/Cube4", typeof(GameObject))) as GameObject;
			pool[i].transform.position = new Vector3(-100, i, 0);
		}

	}

	public void toggleIndicator() {
		IsShowingIndicator = !IsShowingIndicator;
	}

	public void showIndicator() {
		IsShowingIndicator = true;
	}
	
	public void setSpellIndicator(int[] fromPosition, int[] toPosition, Spell spell) {
		if (!IsShowingIndicator) {
			for (int i = 0; i < refill; i++) {
				pool[i].transform.position = new Vector3(-100, i, 0);
			}
			return;
		}
		int[,] coordinates = spell.Shape.toCoords(fromPosition, toPosition);
		for (int i = 0; i < coordinates.GetLength(0); i++) {
			pool[i].transform.position = new Vector3(coordinates[i,0], 0.02f, coordinates[i,1]);
			Indicator script = pool[i].GetComponent<Indicator>();
			script.changeColour(ColourManager.toColor(spell.SpellColour));

			if (!MapTools.IsOutOfBounds(coordinates[i,0], coordinates[i,1]) &&
			    (GameTools.Map.map_unit_occupy[coordinates[i,0], coordinates[i,1]] != null ||
			    (GameTools.Player.Map_position_x == coordinates[i,0] && GameTools.Player.Map_position_y == coordinates[i,1]) ||
			    GameTools.Base.IsWithinBase(coordinates[i,0], coordinates[i,1]))) {
			
				if (!TilesToAnimate.Contains(i)) {
					TilesToAnimate.Add (i);
				}
			} else {
				if (TilesToAnimate.Contains(i)) {
					TilesToAnimate.Remove(i);
				}
			}
		}
		if (coordinates.GetLength(0) != 0) {
			refill = coordinates.GetLength(0);
		}
		if (RangeIndicator != null) {
			RangeIndicator.updateSpellCoOrds(coordinates);
		}
	}
}
