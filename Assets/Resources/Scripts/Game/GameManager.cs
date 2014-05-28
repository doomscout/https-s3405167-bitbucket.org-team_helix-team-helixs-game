using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//GameManager handles the game state through a state machine.
public class GameManager : MonoBehaviour{

	Statemachine game_manager;
    GameInstance turn_manager;

	Player player;
	PlayerBase Base;
	Shop shop;

    public bool GameStart;
	public bool ResumeGame;
	public bool QuitGame;
	public bool GoNextLevel;

    TileMap map;
	public int NumberOfWins = 0;
	public int ModeOffset = -10;

	private float PlayerStartHP;
	private float PlayerEndHP;

	// Use this for initialization
	void Start () {
		initSM();
		initGui();
        
		GameTools.GM = this;

	}

	void initGui() {
		Instantiate(Resources.Load("Prefabs/Gui/MainMenuPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/WinPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/ShopPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/PausePrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/LosePrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/HelpPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/InventoryPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/HealthBarPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/BattleLogPrefab"));
		Instantiate(Resources.Load("Prefabs/Gui/PlaceBaseTitlePrefab"));
        Instantiate(Resources.Load("Prefabs/Gui/Stats"));
	}

	void ResetBools() {
		GameStart = false;
		ResumeGame = false;
		QuitGame = false;
		GoNextLevel = false;
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
		paused2menu.Transition_Action = new Action(actionTransitionClickedQuit);

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
		state_play.Exit_action = new Action(actionPlayExit);
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
		player = new Player();
		Base = new PlayerBase();
		shop = new Shop();
		NumberOfWins = 0;

		shop.RefreshStock(player);
		this.ResetBools();
		GuiManager.Reset();
		GuiManager.IsShowMainMenu = true;
		GuiManager.IsShowHelp = true;
		GuiManager.IsStillMenu = true;
	}

	void actionMenuRunning() {
		//Remove previous level
		//Load next level
	}

	void actionMenuExit() {
		//cleanup menu
		GameStart = false;
		GuiManager.IsStillMenu = false;

		turn_manager = new GameInstance(player, Base, ModeOffset);
		BattleLog.GetInstance().Restart();
		GameTools.GameCamera.MoveToCenter();
		Debug.Log("actionMenuExit");
		PlayerStartHP = GameTools.Player.Health;
	}

	void actionTransitionClickedPlay() {
		//Reset play variables (SM, field values etc.)
		Debug.Log("actionTransitionClickedPlay");
	}

	void actionPlayRunning() {
        turn_manager.tick();
	}

	void actionPlayExit() {
		GuiManager.IsShowHealthBar = false;
		GuiManager.isShowInventory = false;
        GuiManager.IsShowStat = false;

	}

	void actionWinEntry() {
		//Display Win GUI
		GuiManager.IsShowWin = true;
		GuiManager.IsShowHelp = false;
		PlayerEndHP = GameTools.Player.Health;
		shop.RefreshStock(player);
		NumberOfWins++;
		Debug.Log("actionWinEntry");
	}

	void actionWinRunning() {
		//Remove previous level
		//Load next level
	}

	void actionWinExit() {
		//Cleanup menu
		CleanTools.GetInstance().CleanRemoveLevel();

		//create new game instance
		float change_diff = (PlayerEndHP - PlayerStartHP)/PlayerStartHP * 10;
		change_diff += NumberOfWins;
		turn_manager = new GameInstance(player, Base, (int)change_diff);

		GoNextLevel = false;
		player.ReloadSpell();
		BattleLog.GetInstance().Restart();
		GameTools.GameCamera.MoveToCenter();
		PlayerStartHP = GameTools.Player.Health;
        Debug.Log("actionWinExit");
	}

	void actionTransitionClickedContinue() {
		//Reset play variables (SM, field values etc.)
		Debug.Log("actionTransitionClickedContinue");
	}

	void actionPausedEntry() {
		//Show paused GUI
		Debug.Log("actionPausedEntry");
		ResumeGame = false;
		GuiManager.IsPause = true;
	}


	void actionPausedExit() {
		//cleanup pause menu
		Debug.Log("actionPausedExit");
	}

	void actionLoseEntry() {
		//Display Lose GUI
		Debug.Log("actionLoseEntry");
		GuiManager.IsShowLose = true;

	}

	void actionLoseExit() {
		//Hide GUI
		CleanTools.GetInstance().CleanRemoveAll();
		turn_manager = null;
		player = null;
		Base = null;
		shop = null;
		Debug.Log("actionLoseExit");

	}

	void actionTransitionClickedOkay() {
		//Kill character	(	Why here and not in the entry/exit of lose state?
		//						If the player wants to keep the character, we can use a different transition).
		Debug.Log("actionTransitionClickedOkay");
	}

	void actionTransitionClickedQuit() {
		CleanTools.GetInstance().CleanRemoveAll();
		turn_manager = null;
		player = null;
	}
	
	//dummy functions for trigger_condition
	bool conditionClickedPlay() {
		return GameStart;
	}
	
	bool conditionPressedEsc() {
		return Input.GetKeyUp("escape");
	}

	bool conditionClickedQuit() {
		return QuitGame;
	}

	bool conditionClickedOkay() {
		return QuitGame;
	}

	bool conditionClickedResume() {
		return ResumeGame;
	}

	bool conditionClickedContinue() {
		return GoNextLevel;
	}

	bool conditionWin() {
        return Input.GetKeyDown("left") || turn_manager.NumberOfTurnsUntilWin <= 0;
	}

	bool conditionLose() {
		//return  Input.GetKeyDown("down");
		return Input.GetKeyDown("right") || (turn_manager.IsAnimationDone && GameTools.Player.IsDead() || GameTools.Base.IsDead());
	}

    bool conditionTrue() {
        return true;
    }
}
