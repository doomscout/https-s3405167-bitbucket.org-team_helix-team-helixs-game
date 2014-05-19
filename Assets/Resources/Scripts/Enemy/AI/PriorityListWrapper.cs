using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PriorityListWrapper<T> {

	private Stack<T> stack;
	private Queue<T> queue;
	private bool isStack;
	public int Count {
		get { return isStack?stack.Count:queue.Count;} 
		set {}}

	public PriorityListWrapper(bool isStack) {
		this.isStack = isStack;
		if (isStack) {
			stack = new Stack<T>();
		} else {
			queue = new Queue<T>();
		}
	}

	public T Pop() {
		return isStack?stack.Pop():queue.Dequeue();
	}

	public void Push(T t) {
		if (isStack) {
			stack.Push(t);
		} else {
			queue.Enqueue(t);
		}
	}

	public bool Contains(T t) {
		return isStack?stack.Contains(t):queue.Contains(t);
	}

}
