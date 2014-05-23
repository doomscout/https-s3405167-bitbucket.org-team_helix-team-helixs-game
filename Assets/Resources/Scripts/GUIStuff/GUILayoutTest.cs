using UnityEngine;
using System.Collections;

public class GUILayoutTest : MonoBehaviour {

    public Texture tex;
    void OnGUI() {
        GUILayout.BeginArea(new Rect(Screen.width * 0.5f, 0, Screen.width * 0.5f,  Screen.height * 0.5f ), "BOX!");
       
        GUILayout.BeginVertical();
       
        GUILayout.BeginHorizontal();

        for(int i=0; i<10; i++){
            GUILayout.Button("Spell1");
            GUILayout.Button("HEllo");
                GUILayout.EndHorizontal(); //end row
                GUILayout.BeginHorizontal(); //create new row
           
        }
        
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
