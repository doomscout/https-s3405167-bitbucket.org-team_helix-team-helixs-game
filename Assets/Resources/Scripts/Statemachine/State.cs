using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//This class will not inherit monobehaviour
//State class for statemachine
public class State {

	public List<Transition> List_transitions {get; private set;}
	public Action Entry_action{get;set;}
	public List<Action> Running_actions{get; private set;}
	public Action Exit_action{get;set;}

	public State() {
		Running_actions = new List<Action>();
		List_transitions = new List<Transition>();
	}

	public void addTransition(Transition new_transition) {
		if (new_transition == null) {
			UnityEngine.Debug.LogError("addTrasition - new_transition is null");
			UnityEngine.Debug.LogError(System.Environment.StackTrace);
		}
		List_transitions.Add(new_transition);
	}

	public void addAction(Action new_action) {
		if (new_action == null) {
			UnityEngine.Debug.LogError("addAction - new_action is null");
			UnityEngine.Debug.LogError(System.Environment.StackTrace);
		}
		Running_actions.Add(new_action);
	}
}
