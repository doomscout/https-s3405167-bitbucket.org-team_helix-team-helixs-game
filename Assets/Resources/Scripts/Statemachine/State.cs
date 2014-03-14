using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class will not inherit monobehaviour
//State class for statemachine
public class State {

	private List<Transition> transitions;
	private Action entry_action;
	private List<Action> running_actions;
	private Action exit_action;

	public List<Transition> Transitions {get;set;}
	public Action Entry_action{get;set;}
	public List<Action> Running_actions{get;set;}
	public Action Exit_action{get;set;}

	public State() {
		running_actions = new List<Action>();
		transitions = new List<Transition>();
	}

	public void addTransition(Transition new_transition) {
		if (new_transition == null) {
			UnityEngine.Debug.LogError("addTrasition - new_transition is null");
			UnityEngine.Debug.LogError(System.Environment.StackTrace);
		}
		transitions.Add(new_transition);
	}

	public void addAction(Action new_action) {
		if (new_action == null) {
			UnityEngine.Debug.LogError("addAction - new_action is null");
			UnityEngine.Debug.LogError(System.Environment.StackTrace);
		}
		running_actions.Add(new_action);
	}
}
