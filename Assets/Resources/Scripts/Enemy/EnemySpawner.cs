using UnityEngine;
using System.Collections;

public class EnemySpawner : Cleanable{

	int x;
	int y;
	int MaxRechargeTime;
	int RechargeTime;
	int difficultychange;

	int LevelSpawner;
	public GameObject game_object;

	public EnemySpawner(int x, int y, int playerlevel, int difficultychange) {
		this.x = x;
		this.y = y;
		this.LevelSpawner = playerlevel;
		this.difficultychange = difficultychange;

		MaxRechargeTime = 20/playerlevel + 10;
		RechargeTime = MaxRechargeTime;
		game_object = Object.Instantiate(Resources.Load("Prefabs/FlatCube", typeof(GameObject))) as GameObject;
		game_object.transform.position = new Vector3(x, 0.01f, y);
		CleanTools.GetInstance().SubscribeCleanable(this);
	}

	public void CleanUp() {
		GameObject.Destroy(game_object);
	}

	public void Tick() {
		if (GameTools.GI.NumberOfTurnsUntilWin % 25 == 0) {
			MaxRechargeTime = (int)((float)MaxRechargeTime/1.5f) + difficultychange;
			if (MaxRechargeTime <= 1) {
				MaxRechargeTime = 2;
			}
		}
		if (RechargeTime <= 0) {
			if (GameTools.Map.map_unit_occupy[x,y] != null) {
				return;
			}
			RechargeTime = MaxRechargeTime;
			GameTools.GI.list_live_units.Add (new Unit(x, y, LevelSpawner));
		} else {
			RechargeTime--;
		}
	}

}
