﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnManager {

    bool turn_player, turn_enemy;
    Statemachine turn_manager;

    public TurnManager() {
        initSM();
    }

    public void loop() {
        List<Action> tick_actions = turn_manager.loop();
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

        Transition player2animation = new Transition();
        Transition animation2enemy = new Transition();
        Transition enemy2animation = new Transition();
        Transition animation2player = new Transition();

        player2animation.Trigger_condition = new TriggerCondition(conditionValidInput);
        animation2enemy.Trigger_condition = new TriggerCondition(conditionFinishedAnimation);
        enemy2animation.Trigger_condition = new TriggerCondition(conditionDeterminedActions);
        animation2player.Trigger_condition = new TriggerCondition(conditionFinishedAnimation);

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
        state_animation.addAction(new Action(actionAnimationRunning));
        state_enemy.Entry_action = new Action(actionEnemyEntry);
        state_enemy.Exit_action = new Action(actionEnemyExit);

        turn_manager = new Statemachine(state_player);
    }

    //Actions
    void actionPlayerRunning() {
        //listen to input handlers and verify
    }

    void actionPlayerExit() {
        turn_player = false;
        turn_enemy = true;
    }

    void actionAnimationRunning() {
        //loop over units and display animations
        //perhaps use a bool value in the transitions to check which unit to animate
    }

    void actionEnemyEntry() {
        //Determine enemy actions
    }

    void actionEnemyExit() {
        turn_enemy = false;
        turn_player = true;
    }

    //Conditions
    bool conditionValidInput() {
        return true;
    }

    bool conditionFinishedAnimation() {
        return true;
    }

    bool conditionDeterminedActions() {
        return true;
    }
}
