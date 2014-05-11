using UnityEngine;
using System.Collections;

public class SpellIndicator : Cleanable{
	private GameObject[] pool;
	public bool IsShowingIndicator {get; private set;}
	private int refill = 0;
	private int max_pool_size = 20;
	private CastRangeIndicator RangeIndicator;

	public SpellIndicator(int maxPoolSize) {
		max_pool_size = maxPoolSize;
		initSpellIndicator();
		CleanTools.GetInstance().SubscribeCleanable(this);
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
			Indicator script = pool[i].transform.GetComponent<Indicator>();
			script.TriggerAnimation();
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
			pool[i].transform.position = new Vector3(coordinates[i,0], 0.1f, coordinates[i,1]);
			Indicator script = pool[i].GetComponent<Indicator>();
			script.changeColour(ColourManager.toColor(spell.SpellColour));
		}
		refill = coordinates.GetLength(0);
		if (RangeIndicator != null) {
			RangeIndicator.updateSpellCoOrds(coordinates);
		}
	}
}
