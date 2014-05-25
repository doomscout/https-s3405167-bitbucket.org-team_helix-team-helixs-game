using UnityEngine;
using System.Collections;

public class ShopGUI : MonoBehaviour {
    public Shop shop;
    ItemManager deck;
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
		if (shop == null) {
			shop = GameTools.Shop;
		}
		if (deck == null) {
			deck = GameTools.Player.deckManager;
		}
		if (shapeArray == null) {
			shapeArray = shop.SpellStock[0].Shape.shapeIntArray;
		}
		
        //GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.Box(new Rect(Screen.width * 0.5f, 0,Screen.width * 0.5f, Screen.height ), "Shop" );
        //Shop Display
        float j =1;
        for(int i = 0; i < shop.SpellStock.Count; i++)
        {
            spellColour = shop.SpellStock[i].SpellColour.ToString();
            GUI.Box(new Rect(Screen.width * 0.6f, Screen.height * 0.1f * j, 150f, 50f ), "Spell" + " " + spellColour + "\nPower: " + shop.SpellStock[i].Power);
            shapeArray = shop.SpellStock[i].Shape.shapeIntArray;
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
            if (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.1f * j, 100f, 60f), "Buy Spell("+ shop.SpellStock[i].SpellRating +")"))
            {
                shop.TryToSellSpell(GameTools.Player, shop.SpellStock[i]);
            }
           
            j += 1;
        }

        //Display Player's Deck/Inventory
        j = 1;
        GUI.Box(new Rect(0, 0, Screen.width * 0.4f, Screen.height), "Deck/Inventory");
		for(int k = 0; k < deck.deck.Count; k++)
        {
            spellColour = deck.getDeckSpell(k).SpellColour.ToString();
            shapeArray = deck.getDeckSpell(k).Shape.shapeIntArray;
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
            GUI.Box(new Rect(Screen.width * 0.08f, Screen.height * 0.1f * j, 200f, 50f ), "Spell: " + spellColour + "\nPower: " + deck.getDeckSpell(k).Power);
            GUI.Box(new Rect(Screen.width * 0.225f, Screen.height * 0.1f * j, 100, 100f), "" + s);
            if (GUI.Button(new Rect(Screen.width * 0.3f, Screen.height * 0.1f * j, 100f, 50f), "Sell Spell") && deck.deck.Count > 3)
            {
                shop.TryToBuySpell(GameTools.Player, deck.getDeckSpell(k));
            }
            j += 1;
        }
        GUI.Box(new Rect(Screen.width * 0.4f, Screen.height * 0.2f, 150f, 100f), "M - Cursor\nP - PLayer \n * - Areas the spell affects");
		if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 150f, 50f), "Leave")) {
			GuiManager.IsShowShop = false;
			GuiManager.IsShowWin = true;
		}
    }
}
