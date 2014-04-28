using System;
using System.Collections.Generic;

public class NeuralNetwork {
	
	List<List<NN_Node>> layers;

	const int BIAS_VALUE = 1;
	const double LEARNING_RATE = 0.06;
	
	double numInputs;
	double numOutputs;
	double numNodesPerHLayer;
	double numHLayer;
	
	public NeuralNetwork(int _numInputs, int _numOutputs, int _numNodesPerHLayer, int _numHLayer) {
		Random r = new Random();
		layers = new List<List<NN_Node>>();
		
		List<NN_Node> inputLayer = new List<NN_Node>();
		for (int i = 0; i < _numInputs; i++) {
			inputLayer.Add(new NN_Node());
		}
		layers.Add(inputLayer);
		
		for (int i = 0; i < _numHLayer; i++) {
			List<NN_Node> hiddenLayer = new List<NN_Node>();
			for (int j = 0; j < _numNodesPerHLayer; j++) {
				/* Weights between the input layer and the hidden layer */
				if (i == 0) {
					/* +1 for Bias node */
					hiddenLayer.Add(new NN_Node(_numInputs + 1, r));
				/* Weights between the hidden layers */
				} else {
					/* +1 for Bias node */
					hiddenLayer.Add(new NN_Node(_numNodesPerHLayer + 1, r));	
				}
			}
			layers.Add(hiddenLayer);
		}
		
		List<NN_Node> outputLayer = new List<NN_Node>();
		for (int i = 0; i < _numOutputs; i++) {
			outputLayer.Add (new NN_Node(_numNodesPerHLayer + 1, r));
		}
		layers.Add(outputLayer);

		numInputs = _numInputs;
		numOutputs = _numOutputs;
		numNodesPerHLayer = _numNodesPerHLayer;
		numHLayer = _numHLayer;
	}
	
	public double getNodeVal() {
		return (layers[layers.Count-1][0].getInput());
	}
	
	//TRAIN NETWORK USING BACK PROPAGATION
	public void trainNetwork(List<double> _input, List<double> _output) {
    	bool isAble = feedForward(_input);
		
		if (!isAble) {
			Console.WriteLine("Unable to train network -- Input length doesn't match");
			return;
		}
		
		if (_output.Count != numOutputs) {
			Console.WriteLine("Unable to train network -- Output length doesn't match");
			throw new Exception();
		}
		
		for (int i = layers.Count - 1; i >= 1; i--) {
			List<NN_Node> currLayer = layers[i];
            List<NN_Node> prevLayer = layers[i - 1];
			
            //CALCULATE ERROR
            /*Calculate between output layer and hidden layer*/
            if (i == layers.Count - 1) {
                for (int k = 0; k < currLayer.Count; k++) {
                    //Error between calculated answer vs sample answer
                    double error = (_output[k] - currLayer[k].getActivation());
                    currLayer[k].setError(error);
                }
            /*Calculate between hidden layers as well as input layer*/
            } else {
                List<NN_Node> nextLayer = layers[i + 1];
                for (int j = 0; j < currLayer.Count; j++) {
                    double sum = 0;
                    for (int k = 0; k < nextLayer.Count; k++) {
                        sum += nextLayer[k].getError() * nextLayer[k].getWeight(j + 1);
                    }
                    sum *= currLayer[j].getActivation() * (1 - currLayer[j].getActivation());
                    currLayer[j].setError(sum);
                }
            }
            //END CALCULATE ERROR---
            //BACK PROPAGATION
            for (int k = 0; k < currLayer.Count; k++) {
                for (int j = 0; j < currLayer[k].getNumWeights(); j++) {
					double weightAdjust;
					/* Bias unit */
					if (j == 0) {
						weightAdjust = currLayer[k].getError() 
                                    * BIAS_VALUE /*- 0.9 * currLayer[k].getMomentum(j)*/;
					/* Every other node */
					} else {
						weightAdjust = currLayer[k].getError() 
                                    * prevLayer[j - 1].getActivation()/* - 0.9 * currLayer[k].getMomentum(j)*/;
					}
                    //weightAdjust is a vector with same size as weights
					currLayer[k].setMomentum(j, weightAdjust);
                    currLayer[k].addWeight(j, LEARNING_RATE * weightAdjust);
                }
            }
            //END BACK PROPAGATION---
		}
	}
    //FEED FORWARD THROUGH NEURAL NETWORK
    public bool feedForward(List<double> _input) {
       	/* Invalid input size */
		if (_input.Count != numInputs) {
			return false;
		}
		/* Populate input layer with data */
		for (int i = 0; i < _input.Count; i++) {
			layers[0][i].setActivation(_input[i]);
		}
		//FORWARD PROPAGATION
		for (int i = 1; i < layers.Count; i++) {
			List<NN_Node> currLayer = layers[i];
			List<NN_Node> prevLayer = layers[i - 1];
			for (int j = 0; j < currLayer.Count; j++) {
				double netInput = 0;
				currLayer[j].resetActivation();
				for (int k = 0; k < prevLayer.Count; k++) {
					netInput += currLayer[j].getWeight(k + 1) *
												prevLayer[k].getActivation();
				}
                /* Include bias */
                netInput += currLayer[j].getWeight(0) * BIAS_VALUE;
				currLayer[j].setInput(netInput);
				currLayer[j].setActivation(tanSigmoid(netInput));
			}
		}
		//FORWARD PROPAGATION END
		return true;
    }
	
	public void gradientChecking(List<List<double>> _sampleInput, List<List<double>> _sampleOutput){
		double smallVal = 0.0001;
		double threshold = 0.1;
		for (int i = 1; i < layers.Count; i++) {
			List<NN_Node> currLayer = layers[i];
			for (int j = 0; j < currLayer.Count; j++) {
				NN_Node node = currLayer[j];
				for (int k = 0; k < node.getNumWeights(); k++) {
					node.addWeight(k, -smallVal);
					double smallCost = costFunction(_sampleInput, _sampleOutput);
					node.addWeight(k, smallVal * 2);
					double largeCost = costFunction(_sampleInput, _sampleOutput);
					double gradApprox = (largeCost - smallCost)/(2 * smallVal);
					//if ((node.getMomentum(k) - gradApprox) > threshold) {
						Console.WriteLine("Large gradient difference in gradientChecking " + (node.getMomentum(k) - gradApprox));
					//}
					node.addWeight(k, -smallVal);
				}
			}
		}
	}
	
	public double costFunction(List<List<double>> _inputs, List<List<double>> _outputs) {
		double sumCost = 0;
		for (int i = 0; i < _inputs.Count; i++) {
			List<double> input = _inputs[i];
			List<double> output = _outputs[i];
			feedForward (input);
			for (int j = 0; j < layers[layers.Count - 1].Count; j++) {
				double h = layers[layers.Count - 1][j].getActivation();
				double J = (output[j] * Math.Log(h)) + (1 - output[j]) * Math.Log(1 - h);
				sumCost += J;
			}
		}
		return (-1) * sumCost / _inputs.Count;
	}
	
	public List<double> calculateOutput(List<double> _input) {
		List<double> output = new List<double>();
		feedForward(_input);
		int indexOutputLayer = layers.Count - 1;
		for (int i = 0; i < layers[indexOutputLayer].Count; i++) {
			double ans = layers[indexOutputLayer][i].getActivation();
			output.Add(ans);
		}
		return output;
	}
	
	//HELPER FUNCTION
	/* Maps the net input to the sigmoid function */
	private double sigmoid(double _netInput) {
		return 1.0 / (1.0 + Math.Pow(Math.E, -(_netInput)));
	}
	
	public double tanSigmoid(double _netInput) {
		if (_netInput > 500) {
			_netInput = 500;
		}
		if (_netInput < -500) {
			_netInput = 500;
		}
		double top = (Math.Pow(Math.E, (_netInput)) - Math.Pow(Math.E, -(_netInput)));
		double bot = (Math.Pow(Math.E, (_netInput)) + Math.Pow(Math.E, -(_netInput)));
		double val = top / bot;
		if (double.IsNaN(val)) {
			Console.WriteLine("NAN is tansigmoid");
			Console.WriteLine("Input is " + _netInput);
			Console.WriteLine("Top is " + top);
			Console.WriteLine("Bot is " + bot);
		}
		return val;
	}

	public List<List<double>> outputWeights() {
		List<List<double>> doubleOutput = new List<List<double>>();
		List<double> parameters = new List<double>() {
			numInputs,
			numOutputs,
			numNodesPerHLayer,
			numHLayer
		};
		doubleOutput.Add (parameters);
		for (int k = 1; k < layers.Count; k++) {
			List<NN_Node> currLayer = layers[k];
			for (int i = 0; i < currLayer.Count; i++) {
				List<double> allWeights = currLayer[i].getAllWeights();
				doubleOutput.Add(allWeights);
			}
		}
		return doubleOutput;
	}
}


