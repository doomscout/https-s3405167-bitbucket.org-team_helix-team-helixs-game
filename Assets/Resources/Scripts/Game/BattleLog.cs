using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleLog {

	private static BattleLog instance;
	public Queue<string> Log {get; set;}
	public int MaxSize {get; set;}

	private BattleLog() {
		Log = new Queue<string>();
		for (int i = 0; i < 15; i++) {
			Log.Enqueue("");
		}
		MaxSize = 16;
	}

	public static BattleLog GetInstance() {
		if (instance == null) {
			instance = new BattleLog();
		}
		return instance;
	}

	public void AddMessage(string message) {
		Log.Enqueue(message);
		if (Log.Count > MaxSize) {
			Log.Dequeue();
		}
	}
}
