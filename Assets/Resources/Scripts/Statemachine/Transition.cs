using UnityEngine;
using System.Collections;

//This class will not inherit monobehaviour
public class Transition {

	private State target_state;
	private Action action;
	public TriggerCondition trigger_condition;	//Made public since it's easier when using delegates

	public Action Action{get;set;}
	public State Target_state{get;set;}

	public bool isTriggered() {
		if (trigger_condition == null) {
			UnityEngine.Debug.LogError("isTriggered - trigger_condition is null");
			UnityEngine.Debug.LogError(System.Environment.StackTrace);
		}
		return trigger_condition();
	}
}
