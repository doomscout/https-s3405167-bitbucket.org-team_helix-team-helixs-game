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
            GUI.Box(new Rect(Screen.width * 0.6f, Screen.height * 0.1f * j, 150f, 50f ), "Spell" + shop.SpellStock[i].Shape.SpellShape + " " + spellColour);
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
            if (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.1f * j, 100f, 50f), "Buy Spell"))
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
            GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.1f * j, 300f, 50f ), "Spell" + deck.getDeckSpell(k).Shape.SpellShape + " " + spellColour);
           
            if (GUI.Button(new Rect(Screen.width * 0.3f, Screen.height * 0.1f * j, 100f, 50f), "Sell Spell"))
            {
                shop.TryToBuySpell(GameTools.Player, deck.getDeckSpell(k));
            }
            j += 1;
        }
		if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.4f, 100f, 50f), "Leave")) {
			GuiManager.IsShowShop = false;
			GuiManager.IsShowWin = true;
		}
    }
}
