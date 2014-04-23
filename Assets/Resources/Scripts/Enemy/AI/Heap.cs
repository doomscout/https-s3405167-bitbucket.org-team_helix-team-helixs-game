using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

public class Heap<T> {
	
	private List<T> heap;
	private Dictionary<T, int> dictionary;
	private IComparer comparer;
	
	public Heap(){
		heap = new List<T>();
		comparer = Comparer.Default;
		dictionary = new Dictionary<T, int>();
	}
	
	public Heap(IComparer comparer) : this() {
		this.comparer = comparer;
	}
	
	public void insert(T x){
		heap.Add(x);
		dictionary.Add (x, heap.Count - 1);
		int pos = heap.Count - 1;
		int parentPos = parent(pos);
		bubbleUp(pos, parentPos, x);
	}
	
	public bool remove(T item) {
		if (!dictionary.ContainsKey(item)) {
			//doesn't exist, can't remove
			return false;
		}
		int pos = dictionary[item];
		int rightLeafPos = rightMostLeaf(pos);
		dictionary[heap[rightLeafPos]] = pos;
		bool ans = dictionary.Remove(item);
		heap[pos] = heap[rightLeafPos];
		heap.RemoveAt(rightLeafPos);

		if (!ans || (ans && dictionary.ContainsKey(item))) {
			UnityEngine.Debug.Log ("(cannot)remove and does it have the key still in the dictionary?" + dictionary.ContainsKey(item));
		}

		if (rightLeafPos == pos) {
			return true;
		}
		int[] childrenPos = children(pos);
		int parentPos = parent(pos);
		if (childrenPos[0] < 0) {
			//have no children, check parents
			while (comparer.Compare(heap[parentPos], heap[pos]) > 0) {
				swapValuesInHeap(parentPos, pos);
				pos = parentPos;
				parentPos = parent(pos);
			}
		} else {
			//have children, need to check if bubble up or down
			if (compareBoth(heap[pos], childrenPos)) {
				//children is smaller, so bubble down
				while (compareBoth(heap[pos], childrenPos)) {
					int child = getChild(childrenPos);
					swapValuesInHeap(child, pos);
					pos = child;
					childrenPos = children(pos);
					if (childrenPos[0] < 0) {
						break;
					}
				}
			} else if (comparer.Compare(heap[parentPos], heap[pos]) > 0) {
				//current position is smaller then parents, need to move up
				while (comparer.Compare(heap[parentPos], heap[pos]) > 0) {
					swapValuesInHeap(parentPos, pos);
					pos = parentPos;
					parentPos = parent(pos);
				}
			}
		}
		return true;
	}
	
	public T extract(){
		if (heap.Count == 1) {
			T e = heap[0];
			heap.RemoveAt(0);

			if (!dictionary.Remove(e)) {
				UnityEngine.Debug.LogError("Unable to remove1");
			}
			return e;
		}
		T extracted = heap[0];
		dictionary[heap[heap.Count - 1]] = 0;
		dictionary.Remove(extracted);		
		heap[0] = heap[heap.Count - 1];
		heap.RemoveAt(heap.Count - 1);


		int parent = 0;
		int[] childrenPos = children(parent);
		if (childrenPos[0] < 0) {
			//do nothing
		} else {
			while (compareBoth(heap[parent], childrenPos)) {
				int child = getChild(childrenPos);
				swapValuesInHeap(child, parent);
				parent = child;
				childrenPos = children(parent);
				if (childrenPos[0] < 0) {
					break;
				}
			}
		}
		return extracted;
	}

	public bool contains(T item) {
		return dictionary.ContainsKey(item);
	}

	//Hash codes may be the same, but the item is different
	public T getItem(T item) {
		return heap[dictionary[item]];
	}

	public int length(){
		return heap.Count;
	}
	
	public T peek(){
		if (heap.Count < 1) {
			return default(T);
		}
		return heap[0];
	}
	
	private int parent(int position) {
		int parentPos = -1;
		if (heap.Count!= 0 && position != 0) {
			parentPos = (position-1)/2;
		}
		return parentPos;
	}
	
	private int[] children(int position){
		position++;
		int[] childrenPos = new int[2];
		childrenPos[0] = -1;
		childrenPos[1] = -1;
		if (position * 2  <= heap.Count) {
			childrenPos[0] = position * 2 - 1;
		}
		if (position * 2 + 1 <= heap.Count) {
			childrenPos[1] = position * 2;
		}
		return childrenPos;
	}
	
	private bool hasChildren(int position) {
		return (position + 1) * 2 <= heap.Count;
	}
	
	private int rightMostLeaf(int position) {
		return heap.Count - 1;
	}
	
	private void bubbleUp(int pos, int parentPos, T x){
		if (parentPos < 0) {
			return;
		}
		while (comparer.Compare(heap[parentPos], x) > 0) {
			swapValuesInHeap(pos, parentPos);
			pos = parentPos;
			parentPos = parent(pos);
			if (parentPos < 0) {
				break;
			}
		}
	}
	
	private int getChild(int[] children){
		if (children[1] < 0) {
			return children[0];
		}
		//swapped
		if (comparer.Compare(heap[children[1]], heap[children[0]]) > 0){
			return children[0];
		} else {
			return children[1];
		}
	}
	
	private bool compareBoth(T parentVal, int[] children) {
		if (children[1] < 0) {
			return comparer.Compare(parentVal, heap[children[0]]) > 0;
		}
		return (comparer.Compare(parentVal, heap[children[0]]) > 0) || (comparer.Compare(parentVal, heap[children[1]]) > 0); 
	}
	
	private void swapValuesInHeap (int posX, int posY) {
		T temp = heap[posX];
		heap[posX] = heap[posY];
		heap[posY] = temp;
		dictionary[heap[posX]] = posX;
		dictionary[heap[posY]] = posY;
	}
	
}