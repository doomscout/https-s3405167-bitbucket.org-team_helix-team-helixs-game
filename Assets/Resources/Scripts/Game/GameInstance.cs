using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameInstance : Cleanable {

    bool turn_player, turn_enemy;
    Statemachine turn_manager;
	public List<Unit> all_units {get;set;}
	public List<Unit> list_live_units {get; private set;}
	List<Unit> list_dead_units;		
	public CastRangeIndicator UnitCastIndicator;
	Player player;
	GameObject tileMapPrefab;
	Shop shop;


    private bool validInput = false;
	private bool showDamageIndicators = true;

	public bool IsAnimationDone{get; private set;}

    public GameInstance(Player player) {
		this.player = player;
		this.shop = new Shop();

        list_live_units = new List<Unit>();
		list_dead_units = new List<Unit>();
		all_units = new List<Unit>();

		UnitCastIndicator = new CastRangeIndicator();

        initSM();
		initGame();
		GameTools.GI = this;
		GameTools.All_Units = list_live_units;			
		GameTools.Dead_Units = list_dead_units;	
		CleanTools.GetInstance().SubscribeCleanable(this);
    }

	public void CleanUp() {
		GameTools.All_Units = null;
		GameTools.Dead_Units = null;
		GameTools.GI = null;
		all_units = null;
	}

	public void initGame() {
		tileMapPrefab = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/TileMapPrefab", typeof(GameObject))) as GameObject;
		TileMap script = tileMapPrefab.GetComponent<TileMap>();
		script.init ();
		player.loadIntoGame();
		//populate map with enemies
		for (int i = 0; i < 10; i++) {
			Unit u = new Unit();
			list_live_units.Add(u);
			all_units.Add (u);
		}
	}

	public void saveData() {
		//FileHandler.saveEntityData(player, all_units);
	}

    public void tick() {
		if (Input.GetKeyDown("p")) {
			showDamageIndicators = !showDamageIndicators;
			Debug.Log ("Damage indicators are now " + (showDamageIndicators?"On":"Off"));
		}
        List<Action> tick_actions = turn_manager.calculateActions();
        if (tick_actions == null) {
            Debug.LogError("tick_actions in TurnManager is null");
            return;
        }
        foreach (Action action in tick_actions) {
            action();
        }
    }

    void initSM() {
        State state_player = new State();
        State state_animation = new State();
        State state_enemy = new State();
		State state_start = new State();

        Transition player2animation = new Transition();
        Transition animation2enemy = new Transition();
        Transition enemy2animation = new Transition();
        Transition animation2player = new Transition();
		Transition start2player = new Transition();

        player2animation.Trigger_condition = new TriggerCondition(conditionValidInput);
        animation2enemy.Trigger_condition = new TriggerCondition(conditionFinishedPlayerAnimation);
        enemy2animation.Trigger_condition = new TriggerCondition(conditionDeterminedActions);
        animation2player.Trigger_condition = new TriggerCondition(conditionFinishedEnemyAnimation);
		start2player.Trigger_condition = new TriggerCondition(conditionTrue);

        player2animation.Target_state = state_animation;
        animation2enemy.Target_state = state_enemy;
        enemy2animation.Target_state = state_animation;
        animation2player.Target_state = state_player;
		start2player.Target_state = state_player;

        state_player.addTransition(player2animation);
        state_animation.addTransition(animation2enemy);
        state_enemy.addTransition(enemy2animation);
        state_animation.addTransition(animation2player);
		state_start.addTransition(start2player);

		state_player.Entry_action = new Action(actionPlayerEntry);
        state_player.addAction(new Action(actionPlayerRunning));
        state_player.Exit_action = new Action(actionPlayerExit);
		state_animation.Entry_action = new Action(actionAnimationEntry);
        state_animation.addAction(new Action(actionAnimationRunning));
		state_animation.Exit_action = new Action(actionAnimationExit);
        state_enemy.Entry_action = new Action(actionEnemyEntry);
        state_enemy.Exit_action = new Action(actionEnemyExit);
		state_start.Exit_action = new Action(actionStartExit);

        turn_manager = new Statemachine(state_start);
    }

    //Actions

	void actionStartExit() {

	}

	void actionPlayerEntry() {
		player.Prelogic_tick();
	}
	                        
    void actionPlayerRunning() {
        //listen to input handlers and verify
		player.showIndicator();
		validInput = player.listenInput();
        
		UnitCastIndicator.ShowIndicators();
    }

    void actionPlayerExit() {
		player.showIndicatorAnimation();
		if (showDamageIndicators) {
			//player.showDamageTakenAnimation();
			foreach(Unit u in list_live_units) {
				u.showDamageTakenAnimation();
			}
		}
		UnitCastIndicator.ResetIndicators();
        turn_player = false;
        turn_enemy = true;
		player.FinishedAnimation = false;
        validInput = false;
    }

	void actionAnimationEntry() {
		IsAnimationDone = false;
		//hasRemovedUnits = false;
	}

	//loop over units and display animations
    void actionAnimationRunning() {
		IsAnimationDone = true;

		//Check if the player is dead before animating his inputs
		if (player.IsDead()) {
			//Wow, the player is dead, let's do his death animation only
			player.death_tick();
			IsAnimationDone = player.FinishedAnimation;
			return;
		}

		//Check enemy is dead before animating enemy
		foreach (Unit unit in list_live_units) {
			if (unit.IsDead()) {
				list_dead_units.Add (unit);
			}
		}
		//We'll animate the death of the enemies
		foreach (Unit unit in list_dead_units) {
			list_live_units.Remove(unit);
			unit.death_tick();
			if (!unit.FinishedAnimation) {
				IsAnimationDone = false;
			}
		}
		list_dead_units = new List<Unit>();

		//Player isn't dead, let's run his animation tick
		player.animation_tick();
		IsAnimationDone = player.FinishedAnimation;

		//Animate live enemy units
		foreach (Unit unit in list_live_units) {
			unit.animation_tick();
			if (!unit.FinishedAnimation) {
				IsAnimationDone = false;
			}
		}

		ProjectileManager.getInstance().fireProjectiles();
		if (!ProjectileManager.getInstance().FinishedAnimation) {
			IsAnimationDone = false;
		}
    }

	void actionAnimationExit() {
		//Clear the dead units so we don't animate them again
		list_dead_units = new List<Unit>();

	}

    void actionEnemyEntry() {
        //Determine enemy actions
		foreach (Unit unit in list_live_units) {
			unit.Prelogic_tick();
			unit.logic_tick();
			unit.FinishedAnimation = false;
		}
    }

    void actionEnemyExit() {
        turn_enemy = false;
        turn_player = true;
    }

    //Conditions
    bool conditionValidInput() {
		//listen to input handlers and verify
        //return Input.GetKeyDown("space");
        return validInput;
    }

    bool conditionFinishedPlayerAnimation() {
        //return Input.GetKeyDown("0") && turn_enemy;
		return IsAnimationDone && turn_enemy;
    }

    bool conditionFinishedEnemyAnimation() {
        //return Input.GetKeyDown("0") && turn_player;
		return IsAnimationDone && turn_player;
    }

    bool conditionDeterminedActions() {
        //return Input.GetKeyDown("9");
		return true;
    }

	bool conditionTrue() {
		return true;
	}
}
