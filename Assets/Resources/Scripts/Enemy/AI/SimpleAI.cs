using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//State machine for enemies - Simple ver.
public class SimpleAI {

    //Each enemy unit will get a state machine
    Unit unit;
	Statemachine simple_ai;

    public SimpleAI(Unit enemy) {
        unit = enemy;
		initSM ();
    }

    public void tick() {
        List<Action> list_actions = simple_ai.calculateActions();
        foreach (Action action in list_actions) {
            action();
        }
    }

    void initSM() {
        State state_halt = new State();
		State state_seek = new State();
        State state_attack = new State();
        State state_die = new State();

		Transition halt2seek = new Transition();
        Transition seek2attack = new Transition();
        Transition attack2die = new Transition();
        Transition halt2die = new Transition();
		Transition halt2attack = new Transition();
		Transition attack2seek = new Transition();

		halt2seek.Trigger_condition = new TriggerCondition(transitionInSeekRange);
		seek2attack.Trigger_condition = new TriggerCondition(transitionInAttackRange);
		halt2attack.Trigger_condition = new TriggerCondition(transitionInAttackRange);
        halt2die.Trigger_condition = new TriggerCondition(transitionDie);
        attack2die.Trigger_condition = new TriggerCondition(transitionDie);
		attack2seek.Trigger_condition = new TriggerCondition(transitionOutAttackRange);

		halt2seek.Target_state = state_seek;
        seek2attack.Target_state = state_attack;
        halt2die.Target_state = state_die;
		halt2attack.Target_state = state_attack;
        attack2die.Target_state = state_die;
		attack2seek.Target_state = state_seek;

		halt2attack.Transition_Action = new Action(actionTransitionAttack);
		seek2attack.Transition_Action = new Action(actionTransitionAttack);
		halt2seek.Transition_Action = new Action(actionTransitionSeek);

        state_halt.addTransition(halt2die);
        state_halt.addTransition(halt2seek);
		state_halt.addTransition(halt2attack);
		state_seek.addTransition(seek2attack);
		state_attack.addTransition(attack2die);
		state_attack.addTransition(attack2seek);

		state_halt.addAction(new Action(actionHaltRunning));
		state_seek.addAction(new Action(actionSeekRunning));
		state_attack.addAction(new Action(actionAttackRunning));
		state_die.addAction(new Action(actionDieRunning));

	    simple_ai = new Statemachine(state_halt);
    }

    void actionSeekRunning() {
		unit.determineNextMove();
    }

	void actionTransitionSeek() {
		unit.determineNextMove();
	}

	void actionTransitionAttack() {
		unit.CastMainSpell();
	}

	void actionAttackRunning() {
		unit.CastMainSpell();
	}

    void actionHaltRunning() {
        //do nothing? Maybe move closer
    }

    void actionDieRunning() {
        //Set death animation (state_animation will take care of this)
        //unit.IsDead = true;
        //Signal animation to do cleanup? instead of poll, maybe turn_manager.signalDeath(this)
    }

    bool transitionInSeekRange() {
		return false;//true;
				/*AStar
				.fromPosition(unit.Map_position_x, unit.Map_position_y)
				.euclidianDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) < 8.0f ||
				unit.Max_Health != unit.Health; */
    }

    bool transitionInAttackRange() {
        return GraphSearch
					.fromPosition(unit.Map_position_x, unit.Map_position_y)
					.manhattanDistanceFromTarget(GameTools.Player.Map_position_x, GameTools.Player.Map_position_y) <= unit.MainSpell.CastRange;
	}

	bool transitionOutAttackRange() {
		return !transitionInAttackRange();
	}

    bool transitionDie() {
        return false;
    }

}
