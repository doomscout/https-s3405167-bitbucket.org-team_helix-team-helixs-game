using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Initialise state machine
	//Diagram in the Google drive as "Game_manager_state_machine".
	void initSM() {
		//Create the states for the state machine
		State state_menu = new State();
		State state_play = new State();
		State state_paused = new State();
		State state_win = new State();
		State state_lose = new State();

		//Create the transitions for the states
		Transition menu2play = new Transition();
		Transition play2paused = new Transition();
		Transition play2win = new Transition();
		Transition play2lose = new Transition();
		Transition paused2play = new Transition();
		Transition paused2menu = new Transition();
		Transition win2play = new Transition();
		Transition lose2menu = new Transition();

		//Populate states with actions
		state_play.addAction(new Action(actionRunningPlay));

		//populate transitions with conditions
		menu2play.trigger_condition = new TriggerCondition(conditionClickedPlay);
		play2paused.trigger_condition = new TriggerCondition(conditionPressedEsc);
		play2win.trigger_condition = new TriggerCondition(conditionWin);
		play2lose.trigger_condition = new TriggerCondition(conditionLose);
		paused2play.trigger_condition = new TriggerCondition(conditionClickedResume);
		paused2menu.trigger_condition = new TriggerCondition(conditionClickedQuit);
		win2play.trigger_condition = new TriggerCondition(conditionClickedContinue);
		play2lose.trigger_condition = new TriggerCondition(conditionLose);
		lose2menu.trigger_condition = new TriggerCondition(conditionClickedOkay);


	}

	//-----------------------------
	//Functions for the statemachine
	//------------------------------

	//dummy functions for actions for now
	void actionEntryMenu() {
		//Display Menu GUI
		//Load next level (BONUS: this can be multi-threaded for responsive GUI)
	}

	void actionExitMenu() {
		//cleanup menu
	}

	void actionTransitionClickedPlay() {
		//Reset play variables (SM, field values etc.)
	}

	void actionRunningPlay() {
		//insert ingame state machine here
	}

	void actionEntryWin() {
		//Display Win GUI
		//Destroy previous level	(This can be moved to an exit action)
		//Load next level (BONUS: this can be multi-threaded for responsive GUI)
	}

	void actionExitWin() {
		//Cleanup menu
	}

	void actionTransitionClickedContinue() {
		//Reset play variables (SM, field values etc.)
	}

	void actionEntryPaused() {
		//Show paused GUI
	}

	void actionExitPaused() {
		//cleanup pause menu
	}

	void actionTransitionClickedQuit() {
		//Destroy previous level
		//Save character?
	}

	void actionEntryLose() {
		//Display Lose GUI
		//Destroy previous level
	}

	void actionTransitionClickedOkay() {
		//Kill character	(	Why here and not in the entry/exit of lose state?
		//						If the player wants to keep the character, we can use a different transition).
	}
	
	//dummy functions for trigger_condition
	bool conditionClickedPlay() {
		return true;
	}
	
	bool conditionPressedEsc() {
		return true;
	}

	bool conditionClickedQuit() {
		return true;
	}

	bool conditionClickedOkay() {
		return true;
	}

	bool conditionClickedResume() {
		return true;
	}

	bool conditionClickedContinue() {
		return true;
	}

	bool conditionWin() {
		return true;
	}

	bool conditionLose() {
		return true;
	}
}
