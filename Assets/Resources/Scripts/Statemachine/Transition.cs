using UnityEngine;
using System.Collections;

//This class will not inherit monobehaviour
public class Transition {

	public TriggerCondition Trigger_condition{get;set;}	
	public Action Action{get;set;}						//Change this into a list if needed
	public State Target_state{get;set;}

	public bool isTriggered() {
		if (Trigger_condition == null) {
			UnityEngine.Debug.LogError("isTriggered - trigger_condition is null");
			UnityEngine.Debug.LogError(System.Environment.StackTrace);
		}
		return Trigger_condition();
	}
}
