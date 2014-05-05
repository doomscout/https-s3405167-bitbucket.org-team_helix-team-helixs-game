
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class FileHandler{

	const bool APPEND = true;

	public static void saveEntityData(Player p, List<Unit> all_units) {
		if (p == null || all_units == null) {
			Debug.Log("Unable to save -- player or units are null");
			return;
		}
		StreamWriter writer;
		try {
			writer = new StreamWriter(@"Assets/Resources/Data/PlayerUnitData.txt", APPEND);			
		} catch (DirectoryNotFoundException e) {
			Debug.Log (e.ToString());
			Debug.LogError ("Cannot write to path");


			return;
		}
		writer.WriteLine("----Start----");
		writer.WriteLine("Player:");
		writer.WriteLine("Max Health:" + p.stats.Max_Health);
		writer.WriteLine("Health:" + p.stats.Health);
		writer.WriteLine("Units:");
		foreach (Unit u in all_units) {
			writer.WriteLine("Max Health:" + u.MaxHealth);
			writer.WriteLine("Spell Damage:" + u.unitSpell.Power);
		}

		writer.WriteLine("----End----");
		writer.Close();
		Debug.Log("Saved Stats");
	}


}
