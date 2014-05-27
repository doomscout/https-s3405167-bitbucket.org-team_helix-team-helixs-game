using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {
    public bool inventoryEnabled;
    public int  numberInv;
    public GUISkin skin01;
    public GUISkin skin02;
    public float screenWidth;
    public float screenHeight;
    public int[,] shapeArray;
    string s;
    Inventory inventory;
    ItemManager deck;
	bool IsSpellSelected = false;
    void Update()
    {    

        if (Input.GetKeyDown("i")){
            inventoryEnabled = !inventoryEnabled;
        }
        if (GameTools.Player != null) {
        inventory = GameTools.Player.deckManager.inv;
            deck = GameTools.Player.deckManager;
        }

        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }
 void OnGUI()
    {
		if (!GuiManager.isShowInventory) {
			return;
		}
     	GUI.skin = skin01;
        GUI.skin.box.fontSize = 12;
		if (GameTools.Player != null) {
            //DeckGUI
       
            for(int i = 0; i < 3; i++)
            {
            
            

            if (i == 2)
                {
					if (GameTools.Player.spellIndicator.IsShowingIndicator) {
						GUI.skin = skin02;
						GUI.skin.box.fontSize = 12;
					}
            GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.24f*i,Screen.width * 0.1f,245f),
					        "Current Spell:\n" + 
					        "\nColour: " + deck.peekTopSpell().SpellColour.ToString() + 
					        "\nPower: " + deck.getDeckSpell(0).Power + 
					        "\nCastRange " + deck.deck[0].CastRange +
					        "\nEffect " + deck.deck[0].SpellEffect.EffectName() + 
					        (deck.deck[0].SpellEffect.EffectName()!="None"?
					 		"\nTicks: " + deck.deck[0].SpellEffect.TickCount:""));
                     
                    shapeArray = deck.getDeckSpell(0).Shape.shapeIntArray;
                    s = "";
                    for(int l = 0;l < shapeArray.GetLength(0); l++)
                    {
                        for (int m = 0; m < shapeArray.GetLength(1); m++)
                        {
                            if (shapeArray[l,m] == 1)
                            {
                                s += "* "; 
                            }
                            else if (shapeArray[l,m] == 0)
                            {
                                s += "  ";
                            }
                            else if (shapeArray[l, m] == -1)
                            {
                                s+="P ";
                            }
                            else {
                                s+= "M ";
                            }
                        }
                        s+= "\n";
                    }
                    GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.616f,Screen.width * 0.1f,120f), "Shape:\n" + s);
					GUI.skin = skin01;
					GUI.skin.box.fontSize = 12;
                }

                else
                {
                    shapeArray = deck.getDeckSpell(2-i).Shape.shapeIntArray;
                    s = "";
                    for(int l = 0;l < shapeArray.GetLength(0); l++)
                    {
                        for (int m = 0; m < shapeArray.GetLength(1); m++)
                        {
                            if (shapeArray[l,m] == 1)
                            {
                                s += "* "; 
                            }
                            else if (shapeArray[l,m] == 0)
                            {
                                s += "  ";
                            }
                            else if (shapeArray[l, m] == -1)
                            {
                                s+="P ";
                            }
                            else {
                                s+= "M ";
                            }
                        }
                        s+= "\n";
                    }
                    GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.24f * i ,Screen.width * 0.1f,210f),
                        	"Colour: " + deck.getDeckSpell(2-i).SpellColour.ToString() + 
					        "\nPower: " + deck.getDeckSpell(2-i).Power + 
					        "\nCastRange " + deck.deck[2-i].CastRange +
					        "\nEffect " + deck.deck[2-i].SpellEffect.EffectName() + 
					        (deck.deck[2-i].SpellEffect.EffectName()!="None"?
					 		"\nTicks: " + deck.deck[2-i].SpellEffect.TickCount:""));
                    if (i == 1)
                    {
                    GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.34f,Screen.width * 0.1f,120f), "Shape:\n" + s);
                    }
                    else
                    {
                        GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.10f,Screen.width * 0.1f,120f), "Shape:\n" + s);
                    }
                }
            }
           

            /*if(GUI.Button(new Rect((Screen.width * 0.85f), Screen.height * 0.36f, 150f, 30f ), "Add Spell to Inevntory"))
            {
                if (deck.deck.Count > 3){
                GameTools.Player.deckManager.moveSpellDeckToInventory( GameTools.Player.deckManager.getDeckSpell(0));
                }
             
            }
            */
            //Displays Player Money
            GUI.Box(new Rect(0,120f, 115f, 35f), "Money: " + GameTools.Player.Money);
          /* GUI.skin = skin02;
            if (inventoryEnabled){
                //Debug.Log("pressed i");
                if (inventory.size != 0)
                {
                    //InventoryGUI
            GUI.Box(new Rect(0, 300, 120, 500),"Inventory\nCurrent Spell\nShape: " + GameTools.Player.deckManager.getInvSpell(numberInv).Shape.SpellShape 
                            + "\nColour: " + GameTools.Player.deckManager.getInvSpell(numberInv).SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getInvSpell(numberInv).Power);
                
                if (GUI.Button(new Rect(0, 500, 120, 30), "Add Spell to Deck"))
                 {
                    GameTools.Player.deckManager.moveSpellInventoryToDeck(GameTools.Player.deckManager.getInvSpell(0));
                 }
                    //Arrow Buttons to Change Cards
                    if (GUI.Button(new Rect( 0, 530, 20, 20), "<-"))
                    {
                       
                        if (numberInv != 0)
                        {
                            numberInv--;
                        }
                        else{
                            numberInv = inventory.size -1;
                        }
                     }
                    if (GUI.Button(new Rect( 100, 530, 20, 20), "->"))
                    {
                        if (numberInv != inventory.size -1)
                        {
                            numberInv++;
                        }
                        else{
                            numberInv = 0;
                        }
                    }
                }
                else{
                    GUI.Box(new Rect(0, 300, 120, 500),"Inventory");
                
                }
		}
  */      

    }
}
}