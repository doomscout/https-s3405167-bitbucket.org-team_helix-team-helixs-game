using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//State machine for enemies - Simple ver.
public class SimpleAI {

    //Each enemy unit will get a state machine
    Unit unit;
	Statemachine simple_ai;

    public SimpleAI(Unit enemy) {
        unit = enemy;
    }

    public void tick() {
        List<Action> list_actions = simple_ai.calculateActions();
        foreach (Action action in list_actions) {
            action();
        }
    }

    void initSM() {
        State state_halt = new State();
        State state_attack = new State();
        State state_die = new State();

        Transition halt2attack = new Transition();
        Transition attack2halt = new Transition();
        Transition attack2die = new Transition();
        Transition halt2die = new Transition();

        halt2attack.Trigger_condition = new TriggerCondition(transitionInRange);
        attack2halt.Trigger_condition = new TriggerCondition(transitionOutRange);
        halt2die.Trigger_condition = new TriggerCondition(transitionDie);
        attack2die.Trigger_condition = new TriggerCondition(transitionDie);

        halt2attack.Target_state = state_attack;
        attack2halt.Target_state = state_halt;
        halt2die.Target_state = state_die;
        attack2die.Target_state = state_die;

        state_halt.addTransition(halt2die);
        state_halt.addTransition(halt2attack);
        state_attack.addTransition(attack2die);
        state_attack.addTransition(attack2halt);

        simple_ai = new Statemachine(state_halt);
    }

    void actionAttackRunning() {
        //Path finding
        //Set the next move from Up, Down, Left Right (state_animation will take care of this)
    }

    void actionHaltRunning() {
        //return?
    }

    void actionDieRunning() {
        //Set death animation (state_animation will take care of this)
        //unit.IsDead = true;
        //Signal animation to do cleanup? instead of poll, maybe turn_manager.signalDeath(this)
    }

    bool transitionInRange() {
        return true;
    }

    bool transitionOutRange() {
        return true;
    }

    bool transitionDie() {
        return true;
    }

}
