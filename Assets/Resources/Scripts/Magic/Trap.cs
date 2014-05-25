using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trap : Cleanable{

	public Spell spell;
	public int x;
	public int y;
	public int count;
	public bool HasDetonated;

	public GameObject game_object;

	public Trap(Spell s, int x, int y) {
		this.spell = s;
		this.x = x;
		this.y = y;
		HasDetonated = false;

		Init();
	}

	private void Init() {
		if (MapTools.IsOutOfBounds(x, y)) {
			return;
		}
		if (GameTools.Base.IsWithinBase(x,y)) {
			return;
		}
		int tileTrapCount = GameTools.Map.TrapData[x, y].Count;
		if (tileTrapCount > 0) {
			if (GameTools.Map.TrapData[x, y][0].spell.SpellColour != this.spell.SpellColour) {
				for (int j = 0; j < tileTrapCount; j++) {
					GameTools.Map.TrapData[x, y][j].Destroy();
				}
				GameTools.Map.TrapData[x, y] = new List<Trap>();
			}
		}
		this.count = GameTools.Map.TrapData[x, y].Count;
		GameTools.Map.TrapData[x,y].Add (this);

		InitGameObject();
		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	private void InitGameObject() {
		if (game_object == null) {
			game_object = Object.Instantiate(Resources.Load("Prefabs/Cube4", typeof(GameObject))) as GameObject;
		}
		game_object.transform.position = new Vector3(x, ((float)count)/10.0f, y);
		Indicator script = game_object.GetComponent<Indicator>();
		script.changeColour(ColourManager.toColor(spell.SpellColour));
	}

	public void Detonate() {
		if (HasDetonated) {
			return;
		}
		HasDetonated = true;
		GameTools.Map.TrapData[x,y].Remove(this);
		for (int i = 0; i < GameTools.Map.TrapData[x, y].Count; i++) {
			GameTools.Map.TrapData[x, y][i].Detonate();
		}
		if (GameTools.Map.map_unit_occupy[x,y] != null) {
			GameTools.Map.map_unit_occupy[x,y].GetHitByMagic(spell);
		}
		if (x == GameTools.Player.Map_position_x && y == GameTools.Player.Map_position_y) {
			GameTools.Player.GetHitByMagic(spell);
		}
		Indicator script = game_object.transform.GetComponent<Indicator>();
		script.TriggerAnimation();
		GameTools.Map.TrapData[x,y] = null;
		DetonateNeighbours();
	}

	public void Destroy() {
		GameObject.Destroy(game_object);
	}

	public void CleanUp() {
		GameObject.Destroy(game_object);
	}

	private void DetonateNeighbours() {
		int newX, newY;
		newX = x+1;
		newY = y;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (GameTools.Map.TrapData[newX,newY].Count > 0) {
				if (GameTools.Map.TrapData[newX,newY][0].spell.SpellColour == this.spell.SpellColour) {
					GameTools.Map.TrapData[newX,newY][0].Detonate();
				}
			}
		}
		newX = x-1;
		newY = y;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (GameTools.Map.TrapData[newX,newY].Count > 0) {
				if (GameTools.Map.TrapData[newX,newY][0].spell.SpellColour == this.spell.SpellColour) {
					GameTools.Map.TrapData[newX,newY][0].Detonate();
				}
			}
		}
		newX = x;
		newY = y+1;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (GameTools.Map.TrapData[newX,newY].Count > 0) {
				if (GameTools.Map.TrapData[newX,newY][0].spell.SpellColour == this.spell.SpellColour) {
					GameTools.Map.TrapData[newX,newY][0].Detonate();
				}
			}
		}
		newX = x;
		newY = y-1;
		if (!MapTools.IsOutOfBounds(newX, newY)) {
			if (GameTools.Map.TrapData[newX,newY].Count > 0) {
				if (GameTools.Map.TrapData[newX,newY][0].spell.SpellColour == this.spell.SpellColour) {
					GameTools.Map.TrapData[newX,newY][0].Detonate();
				}
			}
		}
	}
}
