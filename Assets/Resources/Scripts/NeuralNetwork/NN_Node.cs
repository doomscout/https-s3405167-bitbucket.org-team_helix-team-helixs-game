using System;
using System.Collections.Generic;

public class NN_Node {
	
	List<double> weights;
	List<double> dWeightsAdjust;
	double dActivation;
	double dError;
	
	List<double> momentums;
	
	double netInput;
	
	double dGradApprox;
	
	int maxVal;
	
	//INPUT LAYER CONSTRUCTOR
	public NN_Node() {
		dActivation = 0;
		dError = 0;
	}
	
	//HIDDEN AND OUTPUT LAYER CONSTRUCTOR
	public NN_Node(int _numWeights, Random r) : this(){
		
		this.weights = new List<double>();
		this.dWeightsAdjust = new List<double>();
		this.momentums = new List<double>();
		this.maxVal = 80;
		for (int i = 0; i < _numWeights; i++) {
			double val = r.Next(-maxVal, maxVal)/100.0; /* range from [-0.30, 0.30] */
			//Console.WriteLine(val);
			weights.Add (val);
			dWeightsAdjust.Add(0);
			momentums.Add (1);
		}
	}
	
	public double getInput() {
		return netInput;
	}
	
	public void setInput(double n){
		netInput = n;
	}
	
	public double getActivation() {
		return dActivation;
	}
	
	public void setActivation(double _val) {
		this.dActivation = _val;
	}
	
	public void resetActivation() {
		this.dActivation = 0;
	}
	
	public void setError(double _err) {
		this.dError = _err;
	}
	
	public double getError() {
		return this.dError;
	}
	
	public void addWeight(int _index, double _weight) {
		this.weights[_index] += _weight;
	}
	
	public double getWeight(int _index) {
		return this.weights[_index];
	}
	
	public int getNumWeights() {
		return this.weights.Count;
	}
	
	public void setMomentum (int j, double _momentum) {
		this.momentums[j] = _momentum;
	}
	
	public double getMomentum(int j) {
		return this.momentums[j];
	}
	
	public void setGradApprox(double _gradApprox) {
		this.dGradApprox = _gradApprox;
	}
	
	public double getGradApprox() {
		return this.dGradApprox;
	}

	public List<double> getAllWeights() {
		return this.weights;
	}
}


