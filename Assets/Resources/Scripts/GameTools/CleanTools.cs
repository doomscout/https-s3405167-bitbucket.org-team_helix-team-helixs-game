using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CleanTools {

	private static CleanTools ct;

	private List<Cleanable> list;

	private CleanTools() {
		list = new List<Cleanable>();
	}

	public static CleanTools GetInstance() {
		if (ct == null) {
			ct = new CleanTools();
		}
		return ct;
	}

	public void SubscribeCleanable(Cleanable c) {
		list.Add(c);
	}

	public void CleanRemove() {
		Cleanable[] array = list.ToArray();
		int count = array.Length;
		for (int i = 0; i < count; i++) {
			array[i].CleanUp();
		}
		list = new List<Cleanable>();
	}
}
