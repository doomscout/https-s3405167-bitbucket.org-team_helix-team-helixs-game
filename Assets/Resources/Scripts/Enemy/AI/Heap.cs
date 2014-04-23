using System.Collections;

//TODO: Change array into a list
public class Heap<T> {
	
	T[] heap;
	IComparer comparer;
	
	public Heap(IComparer comparer) {
		heap = new T[0];
		this.comparer = comparer;
	}
	
	public int parent(int position){
		int parentPos = -1;
		if (heap.Length != 0 && position != 0) {
			parentPos = (position-1)/2;
		}
		return parentPos;
	}
	
	public int[] children(int position){
		position++;
		int[] childrenPos = new int[2];
		childrenPos[0] = -1;
		childrenPos[1] = -1;
		if (position * 2  < heap.Length) {
			childrenPos[0] = position * 2 - 1;
		}
		if (position * 2 + 1 < heap.Length) {
			childrenPos[1] = position * 2;
		}
		return childrenPos;
	}
	
	public void insert(T x){
		T[] temp = new T[heap.Length + 1];
		for (int i = 0 ; i < heap.Length; i++) {
			temp[i] = heap[i];
		}
		temp[heap.Length] = x;
		int pos = heap.Length;
		int parentPos = parent(pos);
		bubbleUp(temp, pos, parentPos, x); 
		heap = temp;
	}
	
	public void bubbleUp(T[] temp, int pos, int parentPos, T x){
		if (parentPos < 0) {
			return;
		}
		
		while (comparer.Compare(temp[parentPos], x) > 0) {
			temp = swap(temp, pos, parentPos);
			pos = parentPos;
			parentPos = parent(pos);
			if (parentPos < 0) {
				break;
			}
		}
	}
	
	public T extract(){
		if (heap.Length == 1) {
			T e = heap[0];
			heap = new T[0];
			return e;
		}
		T extracted = heap[0];
		T[] temp = new T[heap.Length - 1];
		for (int i = 1; i < heap.Length - 1; i++) {
			temp[i] = heap[i];
		}
		temp[0] = heap[heap.Length - 1];
		int parent = 0;
		int[] childrenPos = children(parent);
		if (childrenPos[0] < 0) {
			//do nothing
		} else {
			while (compareBoth(temp, temp[parent], childrenPos)) {
				int child = getChild(temp, childrenPos);
				swap(temp, child, parent);
				parent = child;
				childrenPos = children(parent);
				if (childrenPos[0] < 0) {
					break;
				}
			}
		}
		heap = temp;
		return extracted;
	}
	
	public int getChild(T[] temp, int[] children){
		if (children[1] < 0) {
			return children[0];
		}
		//swapped
		if (comparer.Compare(temp[children[1]], temp[children[0]]) > 0){
			return children[0];
		} else {
			return children[1];
		}
	}
	
	public void printAll(){
		//System.out.println("Printing...");
		for (int i = 0; i < heap.Length; i++) {
			//System.out.println(heap[i].order);
			//System.out.println(heap[i].item);
		}
	}
	
	public int length(){
		return heap.Length;
	}
	
	public T peek(){
		if (heap.Length < 1) {
			return default(T);
		}
		return heap[0];
	}
	
	public bool compareBoth(T[] temp, T parentVal, int[] children) {
		if (children[1] < 0) {
			return comparer.Compare(parentVal, temp[children[0]]) > 0;
		}
		return (comparer.Compare(parentVal, temp[children[0]]) > 0) || (comparer.Compare(parentVal, temp[children[1]]) > 0); 
	}
	
	public T[] swap (T[] array, int posX, int posY) {
		T temp = array[posX];
		array[posX] = array[posY];
		array[posY] = temp;
		return array;
	}
	/*
	public boolean checkMin(){
		if (heap.Length == 0) {
			return true;
		}
		int x = heap[0];
		boolean allGood = true;
		for (int i = 0; i < heap.Length; i++) {
			if (x > heap[i]) {
				allGood = false;
			}
		}
		return allGood;
	}
	
	public boolean checkMax(){
		if (heap.Length == 0) {
			return true;
		}
		int x = heap[0];
		boolean allGood = true;
		for (int i = 0; i < heap.Length; i++) {
			if (x < heap[i]) {
				allGood = false;
			}
		}
		return allGood;
	}
	*/
	
}