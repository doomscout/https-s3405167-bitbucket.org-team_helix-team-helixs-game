using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleanTools {

	private static CleanTools ct;

	private List<Cleanable> list;
	private List<Cleanable> list_PlayerLinked;

	private CleanTools() {
		list = new List<Cleanable>();
		list_PlayerLinked = new List<Cleanable>();
	}

	public static CleanTools GetInstance() {
		if (ct == null) {
			ct = new CleanTools();
		}
		return ct;
	}
	
	public void SubscribeCleanable(Cleanable c) {
		SubscribeCleanable(c, false);
	}

	public void SubscribeCleanable(Cleanable c, bool isPlayerLinked) {
		if (isPlayerLinked) {
			list_PlayerLinked.Add (c);
		} else {
			list.Add(c);
		}
	}

	public void CleanRemoveAll() {
		CleanRemoveLevel();
		CleanRemovePlayerLinked();
	}

	public void CleanRemoveLevel() {
		Cleanable[] array = list.ToArray();
		int count = array.Length;
		for (int i = 0; i < count; i++) {
			array[i].CleanUp();
		}
		list = new List<Cleanable>();
	}

	private void CleanRemovePlayerLinked() {
		Cleanable[] array = list_PlayerLinked.ToArray();
		int count = array.Length;
		for (int i = 0; i < count; i++) {
			array[i].CleanUp();
		}
		list_PlayerLinked = new List<Cleanable>();
	}
}
