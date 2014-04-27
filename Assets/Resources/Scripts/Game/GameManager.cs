using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//GameManager handles the game state through a state machine.
public class GameManager : MonoBehaviour{

	Statemachine game_manager;
    TurnManager turn_manager;

    GameObject main_menu;
    GameObject pause_menu;
    bool Gameexit;
    bool GamePaused;
    MainMenu temp;

    Pause pauseTemp;

    TileMap map;

	// Use this for initialization
	void Start () {
		initSM();
        turn_manager = new TurnManager();
		GameTools.GM = this;

	}
	
	// Update is called once per frame
	void Update () {
		List<Action> tick_actions = game_manager.calculateActions();
		if (tick_actions == null) {
			Debug.LogError("game_manager tick_actions is null");
			return;
		}
		foreach (Action action in tick_actions) {   
			action();
		}
	}

	//Initialise state machine
	//Diagram in the Google drive as "Game_manager_state_machine".
	//Hardcoded for now (Maybe file parsed in future).
	void initSM() {
		//Create the states for the state machine
		State state_menu = new State();
		State state_play = new State();
		State state_paused = new State();
		State state_win = new State();
		State state_lose = new State();
        State state_start = new State();

		//Create the transitions
		Transition menu2play = new Transition();
		Transition play2paused = new Transition();
		Transition play2win = new Transition();
		Transition play2lose = new Transition();
		Transition paused2play = new Transition();
		Transition paused2menu = new Transition();
		Transition win2play = new Transition();
		Transition lose2menu = new Transition();
        Transition start2menu = new Transition();

		//Give the transitions a target state
		menu2play.Target_state = state_play;
		play2paused.Target_state = state_paused;
		play2win.Target_state = state_win;
		play2lose.Target_state = state_lose;
		paused2play.Target_state = state_play;
		paused2menu.Target_state = state_menu;
		win2play.Target_state = state_play;
		lose2menu.Target_state = state_menu;
        start2menu.Target_state = state_menu;

		//populate transitions with conditions
		menu2play.Trigger_condition = new TriggerCondition(conditionClickedPlay);
		play2paused.Trigger_condition = new TriggerCondition(conditionPressedEsc);
		play2win.Trigger_condition = new TriggerCondition(conditionWin);
		play2lose.Trigger_condition = new TriggerCondition(conditionLose);
		paused2play.Trigger_condition = new TriggerCondition(conditionClickedResume);
		paused2menu.Trigger_condition = new TriggerCondition(conditionClickedQuit);
		win2play.Trigger_condition = new TriggerCondition(conditionClickedContinue);
		play2lose.Trigger_condition = new TriggerCondition(conditionLose);
		lose2menu.Trigger_condition = new TriggerCondition(conditionClickedOkay);
        start2menu.Trigger_condition = new TriggerCondition(conditionTrue);

		//give transition some actions
		menu2play.Transition_Action= new Action(actionTransitionClickedPlay);
		win2play.Transition_Action = new Action(actionTransitionClickedContinue);
		lose2menu.Transition_Action = new Action(actionTransitionClickedOkay);

		//give states the populated transitions
		state_menu.addTransition(menu2play);
		state_play.addTransition(play2paused);
		state_play.addTransition(play2win);
		state_play.addTransition(play2lose);
		state_paused.addTransition(paused2menu);
		state_paused.addTransition(paused2play);
		state_win.addTransition(win2play);
		state_lose.addTransition(lose2menu);
        state_start.addTransition(start2menu);

		//Populate states with actions
		state_menu.Entry_action = new Action(actionMenuEntry);
		state_menu.addAction(new Action(actionMenuRunning));
		state_menu.Exit_action = new Action(actionMenuExit);
		state_play.addAction(new Action(actionPlayRunning));
		state_paused.Entry_action = new Action(actionPausedEntry);
		state_paused.Exit_action = new Action(actionPausedExit);
		state_win.Entry_action = new Action(actionWinEntry);
		state_win.addAction(actionWinRunning);
		state_win.Exit_action = new Action(actionWinExit);
		state_lose.Entry_action = new Action(actionLoseEntry);
		state_lose.Exit_action = new Action(actionLoseExit);

		//start state machine
		game_manager = new Statemachine(state_start);
		if (state_menu.List_transitions == null) {
			Debug.LogError("init transition null");
		}
	}

	//-----------------------------
	//Functions for the statemachine
	//------------------------------

	//dummy functions for actions for now
	void actionMenuEntry() {
		//Display Menu GUI
		Debug.Log("actionMenuEntry");
        main_menu = Instantiate(Resources.Load("Prefabs/MainMenuPrefab")) as GameObject;

	}

	void actionMenuRunning() {
		//Remove previous level
		//Load next level
		Debug.Log("actionMenuRunning");
        temp = main_menu.GetComponent<MainMenu>();
        Gameexit = temp.exit;
        if (Gameexit) {
            Debug.Log("asdasdasd");
        }
	}

	void actionMenuExit() {
		//cleanup menu
		Gameexit = false;
		Debug.Log("actionMenuExit");
		//Temp placement of generate level
		/* Disable this for now
		if (map == null) {
			map = (Instantiate(Resources.Load("Prefabs/NewMap")) as GameObject).GetComponent<TileMap>();
		}
		map.BuildMesh();
		*/
	}

	void actionTransitionClickedPlay() {
		//Reset play variables (SM, field values etc.)
		Debug.Log("actionTransitionClickedPlay");
	}

	void actionPlayRunning() {
		//insert ingame state machine here
		//Debug.Log("actionPlayRunning");
        turn_manager.tick();
	}

	void actionWinEntry() {
		//Display Win GUI
		Debug.Log("actionWinEntry");
	}

	void actionWinRunning() {
		//Remove previous level
		//Load next level
		Debug.Log("actionWinRunning");
	}

	void actionWinExit() {
		//Cleanup menu
		Debug.Log("actionWinExit");
	}

	void actionTransitionClickedContinue() {
		//Reset play variables (SM, field values etc.)
		Debug.Log("actionTransitionClickedContinue");
	}

	void actionPausedEntry() {
		//Show paused GUI
        pause_menu = Instantiate(Resources.Load("Prefabs/PausePrefab")) as GameObject;
        GamePaused = false;
		Debug.Log("actionPausedEntry");
	}


	void actionPausedExit() {
		//cleanup pause menu
       
		Debug.Log("actionPausedExit");
	}

	void actionLoseEntry() {
		//Display Lose GUI
		Debug.Log("actionLoseEntry");
	}

	void actionLoseExit() {
		//Hide GUI
		Debug.Log("actionLoseExit");
	}

	void actionTransitionClickedOkay() {
		//Kill character	(	Why here and not in the entry/exit of lose state?
		//						If the player wants to keep the character, we can use a different transition).
		Debug.Log("actionTransitionClickedOkay");
	}
	
	//dummy functions for trigger_condition
	bool conditionClickedPlay() {
		return Gameexit;
	}
	
	bool conditionPressedEsc() {
		return Input.GetKeyDown("up");
	}

	bool conditionClickedQuit() {
		return Input.GetKeyDown("left");
	}

	bool conditionClickedOkay() {
		return Input.GetKeyDown("left");
	}

	bool conditionClickedResume() {
        Debug.Log("conditionClickedResume");
        pauseTemp =  pause_menu.GetComponent<Pause>();
        return  pauseTemp.resume;
	}

	bool conditionClickedContinue() {
		return Input.GetKeyDown("left");
	}

	bool conditionWin() {
		return  Input.GetKeyDown("right");
	}

	bool conditionLose() {
		//return  Input.GetKeyDown("down");
		return turn_manager.IsAnimationDone && GameTools.Player.IsDead;
	}

    bool conditionTrue() {
        return true;
    }
}
