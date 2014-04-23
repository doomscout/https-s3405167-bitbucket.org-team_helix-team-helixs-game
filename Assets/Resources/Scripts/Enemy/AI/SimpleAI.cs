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

    float distFromPlayer(float x, float y) {
        return Mathf.Sqrt((GameTools.Player.Map_position_x - x) * (GameTools.Player.Map_position_x - x) + 
                          (GameTools.Player.Map_position_y - y) * (GameTools.Player.Map_position_y - y));
    }

	void findNeighbours(AStarNode a) {
		int x = a.CoOrds[0], y = a.CoOrds[1];
		if (x - 1 < 0 || y - 1 < 0 ||
		    x + 1 > GameTools.Map) {

		}
	}

	void findPath() {
		Heap<AStarNode> openSet = new Heap<AStarNode>(new AStarComparer());
		HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
		bool found = false;

		AStarNode currentPosition = new AStarNode(unit.Map_position_x, unit.Map_position_y);

		openSet.insert(currentPosition);

		while (!found) {
			currentPosition = openSet.extract();
		}

	}

    void actionAttackRunning() {
        //Path finding
		/*
        if (unit.Map_position_x + 1 > GameTools.Map.size_x) {
            //don't explore position_x + 1
        }
        if (unit.Map_position_x - 1 < 0) {
            //don't explore position_x -1
        }
        if (unit.Map_position_y + 1 > GameTools.Map.size_z) {
            //don't explore position_y + 1
        }
        if (unit.Map_position_y - 1 < 0) {
            //don't explore position_y - 1
        }
        */
        //TODO: PRIORITY QUEUE to explore the next one in the open set
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
