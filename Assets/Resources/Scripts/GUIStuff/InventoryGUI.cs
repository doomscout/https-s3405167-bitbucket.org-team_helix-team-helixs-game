using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {
    public bool inventoryEnabled;
    public int  numberInv;
    Inventory inventory;
    void Update()
    {
        if (Input.GetKeyDown("i")){
            inventoryEnabled = !inventoryEnabled;
        }
        if (GameTools.Player != null) {
        inventory = GameTools.Player.deckManager.inv;
        }
    }
 void OnGUI()
    {
		if (GameTools.Player != null) {
	        GUI.Box(new Rect(700,200,100,100),"Current Spell:" + "\nShape: " + GameTools.Player.deckManager.peekTopSpell().Shape.spellShape 
                    + "\nColour: " + GameTools.Player.deckManager.peekTopSpell().SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getDeckSpell(0).Power );
	        GUI.Box(new Rect(700,100,100,100),"\nShape: " + GameTools.Player.deckManager.getDeckSpell(1).Shape.spellShape 
                    + "\nColour: " + GameTools.Player.deckManager.getDeckSpell(1).SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getDeckSpell(0).Power );
	        GUI.Box(new Rect(700,0,100,100),"\nShape: " + GameTools.Player.deckManager.getDeckSpell(2).Shape.spellShape 
                    + "\nColour: " + GameTools.Player.deckManager.getDeckSpell(2).SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getDeckSpell(1).Power);

            if(GUI.Button(new Rect(700, 300, 150, 30), "Add Spell to Inevntory"))
            {
                GameTools.Player.deckManager.moveSpellDeckToInventory( GameTools.Player.deckManager.getDeckSpell(0));
            }

            if (inventoryEnabled){
                Debug.Log("pressed i");
                if (inventory.size != 0)
                {
            GUI.Box(new Rect(100, 300, 120, 500),"Inventory\nCurrent Spell\nShape: " + GameTools.Player.deckManager.getInvSpell(numberInv).Shape.spellShape 
                            + "\nColour: " + GameTools.Player.deckManager.getInvSpell(numberInv).SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getInvSpell(numberInv).Power);
                
                if (GUI.Button(new Rect(100, 500, 120, 30), "Add Spell to Deck"))
                 {
                    GameTools.Player.deckManager.moveSpellInventoryToDeck(GameTools.Player.deckManager.getInvSpell(0));
                 }
                    if (GUI.Button(new Rect( 100, 530, 20, 20), "<-"))
                    {
                        if (numberInv != 0)
                        {
                            numberInv--;
                        }
                        else{
                            numberInv = inventory.size -1;
                        }
                     }
                    if (GUI.Button(new Rect( 200, 530, 20, 20), "->"))
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
                    GUI.Box(new Rect(100, 300, 120, 500),"Inventory");
                
                }
		}

    }
}
}