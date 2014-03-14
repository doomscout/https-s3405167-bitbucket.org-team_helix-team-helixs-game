using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Action();
public delegate bool TriggerCondition();

//Generic Statemachine
//Algorithm from Artificial Intelligence for Games Second Edition page 311

//This class will not inherit monobehaviour
public class Statemachine {

	State initial_state;
	State current_state;

	public Statemachine(State inital_state) {
		this.initial_state = inital_state;
		current_state = initial_state;
	}

	//Update for statemachine
	//This is NOT the monobehaviour update function.
	public List<Action> update() {
		List<Action> actions = new List<Action>();
		Transition triggered_transition = null;
		State target_state = null;


		foreach (Transition possible_transition in current_state.Transitions) {
			if (possible_transition.isTriggered()) {
				triggered_transition = possible_transition;
				break;
			}
		}

		if (triggered_transition != null) {
			target_state = triggered_transition.Target_state;

			if (current_state.Exit_action != null) {
				actions.Add(current_state.Exit_action);
			}
			if (triggered_transition.Action != null) {
				actions.Add(triggered_transition.Action);
			}
			if (target_state.Entry_action != null) {
				actions.Add(target_state.Entry_action);
			}

			current_state = target_state;
			return actions;
		}
		return current_state.Running_actions;
	}
}
