using UnityEngine;
using System.Collections;

public enum TileInteraction {Stand, Damage};
public enum TileReward {HP, Money};

public class BonusTile : Cleanable{
	//stand on bonus tile for money
	//stand on tile for hp
	//attack tile for money

	public float hp;
	public int tick;
	public int amount;
	public Colour MainColour;
	public TileInteraction Interaction;
	public TileReward Reward;

	public int x;
	public int y;

	GameObject game_object;

	public BonusTile(int x, int y) {
		hp = Random.Range(1, 20);
		tick = 1;//Random.Range(1, 3);
		amount = (int)hp;
		MainColour = ColourManager.getRandomColour();
		Interaction = TileInteraction.Stand;//(TileInteraction)Random.Range (0, 2);
		Reward = (TileReward)Random.Range(0,2);

		this.x = x;
		this.y = y;

		CleanTools.GetInstance().SubscribeCleanable(this);
		InitGameObject();
	}

	private void InitGameObject() {
		if (game_object == null) {
			if (Interaction == TileInteraction.Damage) {
				game_object = Object.Instantiate(Resources.Load("Prefabs/FlatCube", typeof(GameObject))) as GameObject;
			} else {
				game_object = Object.Instantiate(Resources.Load("Prefabs/Circle", typeof(GameObject))) as GameObject;
			}
		}
		game_object.transform.position = new Vector3(x, 0.01f, y);
		GameTools.Map.BonusTileData[x,y] = this;
		if (Reward == TileReward.HP) {
			game_object.renderer.material.color = Color.red;
		} else {
			game_object.renderer.material.color = Color.yellow;
		}
	}

	public bool IsDead() {
		if (hp <= 0) {
			if (Reward == TileReward.Money){
				GameTools.Player.GetMoney(amount);
				ShowText("+" + amount, Color.yellow, 0);
				Debug.Log ("Gained Money");
			} else {
				GameTools.Player.Health += amount;
				ShowText("+" + amount, Color.blue, 0);
				Debug.Log ("Gained health");
			}
			GameObject.Destroy(game_object);
			GameTools.Map.BonusTileData[x,y] = null;
		}
		return hp <= 0 || tick <= 0;
	}

	public void TickDown(Player p) {
		tick--;
		if (Interaction == TileInteraction.Stand) {
			if (Reward == TileReward.Money){
				GameTools.Player.GetMoney(amount);
				ShowText("+" + amount + " gold", Color.yellow, 0);
				Debug.Log ("Gained money tick");
			} else {
				p.Health += amount;
				ShowText("+" + amount + " HP", Color.red, 0);
				Debug.Log ("Gained health tick");
			}
		}
		if (tick == 0) {
			GameObject.Destroy(game_object);
			GameTools.Map.BonusTileData[x,y] = null;
		}
	}

	public void GetHitByMagic(Spell taken_spell) {
		float modifier = 1.0f;
		if (Interaction == TileInteraction.Damage) {
			if (ColourManager.getWeakness(taken_spell.SpellColour) == MainColour) {
				//The spell is weak against our colour
				modifier = ColourManager.WeaknessModifier;
			}
			
			float dmg = taken_spell.Power * modifier;
			hp -= dmg;
			if (!IsDead()) {
				ShowText("HP remaining: " + hp, ColourManager.toColor(MainColour), 0);
			}
		}
	}

	private void ShowText(string text, Color c, int offset) {
		GameObject o = Object.Instantiate(Resources.Load("Prefabs/DamagePopupPrefab", typeof(GameObject))) as GameObject;
		DamagePopup script = o.GetComponent<DamagePopup>();
		script.setText(text);
		script.setColor(c);
		o.transform.position = new Vector3(game_object.transform.position.x, 0, game_object.transform.position.z + 1.0f + offset/2.0f);
	}

	public void CleanUp() {
		GameObject.Destroy (game_object);
	}
}

