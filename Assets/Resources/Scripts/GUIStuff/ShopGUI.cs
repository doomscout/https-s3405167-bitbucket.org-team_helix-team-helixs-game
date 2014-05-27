using UnityEngine;
using System.Collections;

public class ShopGUI : MonoBehaviour {
    public int[,] shapeArray;
    public string spellColour;
    public GUISkin skin01;
    public GUISkin skin02;
   string s;
	void Start()
    {
        
    }

    void OnGUI()
    {
		if (!GuiManager.IsShowShop) {
			return;
		}
		if (shapeArray == null) {
			shapeArray = GameTools.Shop.SpellStock[0].Shape.shapeIntArray;
		}
        GUI.skin = skin01;
        //GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.Box(new Rect(Screen.width * 0.5f, 0,Screen.width * 0.5f, Screen.height ), "Shop" );
        //Shop Display
        float j =1;
		for(int i = 0; i < GameTools.Shop.SpellStock.Count; i++)
        {
			spellColour = GameTools.Shop.SpellStock[i].SpellColour.ToString();
            GUI.Box(new Rect(Screen.width * 0.6f, Screen.height * 0.1f * j, 150f, 110f ), 
			        "Spell " + spellColour + 
			        "\nPower: " + GameTools.Shop.SpellStock[i].Power +
			        "\nCast Range: " + GameTools.Shop.SpellStock[i].CastRange +
			        "\nEffect " + GameTools.Shop.SpellStock[i].SpellEffect.EffectName() + 
             		(GameTools.Shop.SpellStock[i].SpellEffect.EffectName()!="None"?
                   	"\nTicks: " + GameTools.Shop.SpellStock[i].SpellEffect.TickCount:""));
			shapeArray = GameTools.Shop.SpellStock[i].Shape.shapeIntArray;
            s = "";
            for(int k = 0;k < shapeArray.GetLength(0); k++)
            {
                for (int z = 0; z < shapeArray.GetLength(1); z++)
                {
                    if (shapeArray[k,z] == 1)
                    {
                        s += "*"; 
                    }
                    else if (shapeArray[k,z] == 0)
                    {
                        s += " ";
                    }
                    else if (shapeArray[k, z] == -1)
                    {
                        s+="P";
                    }
                    else {
                        s+= "M";
                    }
                }
                s+= "\n";
            }
            GUI.Box(new Rect(Screen.width * 0.7f, Screen.height * 0.1f * j, 150f, 90f), s);
			if (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.1f * j, 200f, 60f), "Buy Spell("+ GameTools.Shop.SpellStock[i].SpellRating +")"))
            {
				GameTools.Shop.TryToSellSpell(GameTools.Player, GameTools.Shop.SpellStock[i]);
            }
           
            j += 1;
        }

        //Display Player's Deck/Inventory
        j = 1;
        GUI.skin = skin02;
        GUI.Box(new Rect(0, 0, Screen.width * 0.4f, Screen.height), "Deck/Inventory");
		for(int k = 0; k < GameTools.Player.deckManager.deck.Count; k++)
        {
			spellColour = GameTools.Player.deckManager.getDeckSpell(k).SpellColour.ToString();
			shapeArray = GameTools.Player.deckManager.getDeckSpell(k).Shape.shapeIntArray;
            s = "";
            for(int l = 0;l < shapeArray.GetLength(0); l++)
            {
                for (int m = 0; m < shapeArray.GetLength(1); m++)
                {
                    if (shapeArray[l,m] == 1)
                    {
                        s += "*"; 
                    }
                    else if (shapeArray[l,m] == 0)
                    {
                        s += " ";
                    }
                    else if (shapeArray[l, m] == -1)
                    {
                        s+="P";
                    }
                    else {
                        s+= "M";
                    }
                }
                s+= "\n";
            }
            GUI.Box(new Rect(Screen.width * 0.06f, Screen.height * 0.1f * j, 200f, 110f ), 
			        "Spell: " + spellColour + 
			        "\nPower: " + GameTools.Player.deckManager.getDeckSpell(k).Power +
			        "\nCast Range: " + GameTools.Player.deckManager.deck[k].CastRange+
			        "\nEffect " + GameTools.Player.deckManager.deck[k].SpellEffect.EffectName() + 
			        (GameTools.Player.deckManager.deck[k].SpellEffect.EffectName()!="None"?
		 			"\nTicks: " + GameTools.Player.deckManager.deck[k].SpellEffect.TickCount:""));
            GUI.Box(new Rect(Screen.width * 0.18f, Screen.height * 0.1f * j, 100, 100f), "" + s);
			if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.1f * j, 260f, 30f), "Sell Spell") && GameTools.Player.deckManager.deck.Count > 3)
           	{
				GameTools.Shop.TryToBuySpell(GameTools.Player, GameTools.Player.deckManager.getDeckSpell(k));
            }
			if (GUI.Button(new Rect(Screen.width * 0.25f, Screen.height * 0.1f * j + 30, 260f, 30f), "Load Turret with this spell") && GameTools.Player.deckManager.deck.Count > 3)
			{
				GameTools.Base.GetSpellFromPlayer(GameTools.Player.deckManager.deck[k], GameTools.Player);
			}
            j += 1;
        }
        GUI.skin = skin01;
        GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, 150f, 100f), "M - Cursor\nP - PLayer \n * - Areas the spell affects");
        GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.1f, 150f, 30f), "Money: " + GameTools.Player.Money);
		if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.7f, 150f, 50f), "Leave Shop") && GameTools.Player.deckManager.deck.Count >= 3) {
			GuiManager.IsShowShop = false;
			if (GuiManager.IsStillMenu) {
				GameTools.GM.GameStart = true;
				//do nothing
			} else {
				GuiManager.IsShowWin = true;
			}
		}
		if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 150f, 50f), "Up Turret (20)")) {
			Debug.Log ("This button does nothing for now");
		}
		if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.45f, 150f, 50f), "Repair base (3)")) {
			Debug.Log ("This button does nothing for now");
		}
    }
}
