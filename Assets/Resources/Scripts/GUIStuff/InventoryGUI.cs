using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {
    public bool inventoryEnabled;
    public int  numberInv;
    public GUISkin skin01;
    public GUISkin skin02;
    public float screenWidth;
    public float screenHeight;
    Inventory inventory;
    ItemManager deck;
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
      //  GUI.skin = skin01;
		if (GameTools.Player != null) {
            //DeckGUI
            GUI.depth = 20;
           
            GUI.depth = 5;
            GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.12f,100f,100f),"\nShape: " + deck.getDeckSpell(1).Shape.SpellShape 
                    + "\nColour: " + deck.getDeckSpell(1).SpellColour.ToString() + "\nPower: " + deck.getDeckSpell(0).Power );
            GUI.depth = 10;
            GUI.Box(new Rect((Screen.width * 0.85f),0 ,100f,100f),"\nShape: " + deck.getDeckSpell(2).Shape.SpellShape 
                    + "\nColour: " + deck.getDeckSpell(2).SpellColour.ToString() + "\nPower: " + deck.getDeckSpell(1).Power);
           
            GUI.Box(new Rect((Screen.width * 0.85f),Screen.height * 0.24f,100f,100f),"Current Spell:" + "\nShape: " + deck.peekTopSpell().Shape.SpellShape 
                    + "\nColour: " + deck.peekTopSpell().SpellColour.ToString() + "\nPower: " + deck.getDeckSpell(0).Power);
            if(GUI.Button(new Rect((Screen.width * 0.85f), Screen.height * 0.36f, 150f, 30f ), "Add Spell to Inevntory"))
            {
                if (deck.deck.Count > 3){
                GameTools.Player.deckManager.moveSpellDeckToInventory( GameTools.Player.deckManager.getDeckSpell(0));
                }
             
            }
          //  GUI.skin = skin02;
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

    }
}
}