using UnityEngine;
using System.Collections;

public class InventoryGUI : MonoBehaviour {
 void OnGUI()
    {
        GUI.Box(new Rect(700,200,100,100),GameTools.Player.deckManager.peekTopSpell().Shape.spellShape 
                + "\n" + GameTools.Player.deckManager.peekTopSpell().SpellColour.ToString() );
        GUI.Box(new Rect(700,100,100,100),GameTools.Player.deckManager.getSpell(1).Shape.spellShape 
                + "\n" + GameTools.Player.deckManager.getSpell(1).SpellColour.ToString() );
        GUI.Box(new Rect(700,0,100,100),GameTools.Player.deckManager.getSpell(2).Shape.spellShape 
                + "\n" + GameTools.Player.deckManager.getSpell(2).SpellColour.ToString() );

    }
}
