using UnityEngine;
using System.Collections;

public class InputTest : MonoBehaviour {

	void Update() {
		foreach (char c in Input.inputString) {
			if (c == "\b"[0])
				if (guiText.text.Length != 0)
					guiText.text = guiText.text.Substring(0, guiText.text.Length - 1);
			
			else
				if (c == "\n"[0] || c == "\r"[0])
					print("User entered his name: " + guiText.text);
			else
				guiText.text += c;
		}
	}
}