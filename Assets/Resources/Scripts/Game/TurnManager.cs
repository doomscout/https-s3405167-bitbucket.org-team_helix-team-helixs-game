using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager {

    bool turn_player, turn_enemy;
    Statemachine turn_manager;
    List<Unit> list_live_units;
	List<Unit> list_dead_units;							//Remember to reset kill units after both sides' turns

	bool isAnimationDone;
	bool hasRemovedUnits;

    public TurnManager() {
        list_live_units = new List<Unit>();
		list_dead_units = new List<Unit>();
		GameTools.All_Units = list_live_units;
		GameTools.Dead_Units = list_dead_units;			
        initSM();
		GameTools.TM = this;
    }

    public void tick() {
        List<Action> tick_actions = turn_manager.calculateActions();
        if (tick_actions == null) {
            Debug.LogError("tick_actions in TurnManager is null");
            return;
        }
        foreach (Action action in tick_actions) {
            action();
        }
    }

    public void signalDeath(Unit unit) {
		list_dead_units.Add(unit);
    }

    void initSM() {
        State state_player = new State();
        State state_animation = new State();
        State state_enemy = new State();

        Transition player2animation = new Transition();
        Transition animation2enemy = new Transition();
        Transition enemy2animation = new Transition();
        Transition animation2player = new Transition();

        player2animation.Trigger_condition = new TriggerCondition(conditionValidInput);
        animation2enemy.Trigger_condition = new TriggerCondition(conditionFinishedPlayerAnimation);
        enemy2animation.Trigger_condition = new TriggerCondition(conditionDeterminedActions);
        animation2player.Trigger_condition = new TriggerCondition(conditionFinishedEnemyAnimation);

        player2animation.Target_state = state_animation;
        animation2enemy.Target_state = state_enemy;
        enemy2animation.Target_state = state_animation;
        animation2player.Target_state = state_player;

        state_player.addTransition(player2animation);
        state_animation.addTransition(animation2enemy);
        state_enemy.addTransition(enemy2animation);
        state_animation.addTransition(animation2player);

        state_player.addAction(new Action(actionPlayerRunning));
        state_player.Exit_action = new Action(actionPlayerExit);
		state_animation.Entry_action = new Action(actionAnimationEntry);
        state_animation.addAction(new Action(actionAnimationRunning));
        state_enemy.Entry_action = new Action(actionEnemyEntry);
        state_enemy.Exit_action = new Action(actionEnemyExit);

        turn_manager = new Statemachine(state_player);
    }

    //Actions
    void actionPlayerRunning() {
        //listen to input handlers and verify
        Debug.Log("actionPlayerRunning");
    }

    void actionPlayerExit() {
        turn_player = false;
        turn_enemy = true;
        Debug.Log("actionPlayerExit");
    }

	void actionAnimationEntry() {
		isAnimationDone = false;
		hasRemovedUnits = false;
	}

    void actionAnimationRunning() {
        //loop over units and display animations
        //perhaps use a bool value in the transitions to check which unit to animate - nah

		//player.tick();
		isAnimationDone = true;
		foreach (Unit unit in list_live_units) {
			unit.animation_tick();
			if (!unit.FinishedAnimation) {
				isAnimationDone = false;
			}
		}
		foreach (Unit unit in list_dead_units) {
			if (!hasRemovedUnits) {
				list_live_units.Remove(unit);
			}
			unit.death_tick();
			if (!unit.FinishedAnimation) {
				isAnimationDone = false;
			}
		}
		hasRemovedUnits = true;
        Debug.Log("actionAnimationRunning");
    }

	void actionAnimationExit() {
		list_dead_units = new List<Unit>();
	}

    void actionEnemyEntry() {
        //Determine enemy actions
		foreach (Unit unit in list_live_units) {
			unit.determineNextMove();
		}
        Debug.Log("actionEnemyEntry");
    }

    void actionEnemyExit() {
        turn_enemy = false;
        turn_player = true;
        Debug.Log("actionEnemyExit");
    }

    //Conditions
    bool conditionValidInput() {
		//listen to input handlers and verify
        return Input.GetKeyDown("space");
    }

    bool conditionFinishedPlayerAnimation() {
        return Input.GetKeyDown("0") && turn_enemy;
		//return isAnimationDone && turn_enemy;
    }

    bool conditionFinishedEnemyAnimation() {
        return Input.GetKeyDown("0") && turn_player;
		//return isAnimationDone && turn_enemy;
    }

    bool conditionDeterminedActions() {
        return Input.GetKeyDown("9");
    }
}
