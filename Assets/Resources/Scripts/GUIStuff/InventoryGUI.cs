using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {
 void OnGUI()
    {
		if (GameTools.Player != null) {
	        GUI.Box(new Rect(700,200,100,100),"Current Spell:" + "\nShape: " + GameTools.Player.deckManager.peekTopSpell().Shape.spellShape 
                    + "\nColour: " + GameTools.Player.deckManager.peekTopSpell().SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getDeckSpell(0).Power );
	        GUI.Box(new Rect(700,100,100,100),"\nShape: " + GameTools.Player.deckManager.getDeckSpell(1).Shape.spellShape 
                    + "\nColour: " + GameTools.Player.deckManager.getDeckSpell(1).SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getDeckSpell(0).Power );
	        GUI.Box(new Rect(700,0,100,100),"\nShape: " + GameTools.Player.deckManager.getDeckSpell(2).Shape.spellShape 
                    + "\nColour: " + GameTools.Player.deckManager.getDeckSpell(2).SpellColour.ToString() + "\nPower: " + GameTools.Player.deckManager.getDeckSpell(1).Power);

		}

    }
}
