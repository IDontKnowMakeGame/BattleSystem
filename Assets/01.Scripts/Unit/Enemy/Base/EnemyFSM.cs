using System.Collections;
using System.Collections.Generic;
using Unit.Enemy.AI;
using Unit.Enemy.AI.State;
using UnityEditor.Rendering;
using UnityEngine;
using Behaviour = Unit.Behaviour;

public class EnemyFSM : Behaviour
{
    private AIState currentState;
    private AIState nextState = null;
    private readonly Dictionary<string, AIState> states = new Dictionary<string, AIState>();
    
    
    public bool ChangeState(string state)
    {
        if (states.ContainsKey(state) == false)
        {
            Debug.LogError("State " + state + " does not exist");
            return false;
        }
        currentState = states[state];
        nextState = null;
        return true;
    }

    private AIState AddState(string state, AIState aiState)
    {
        if (states.ContainsKey(state) == false)
        {
            aiState.Awake();
            states.Add(state, aiState);
        }

        return aiState;
    }

    public override void Awake()
    {
        var roamingState = new RoamingState();
        currentState = AddState(roamingState.Name, roamingState);
    }

    public override void Update()
    {
        nextState = currentState.ExcuteState();
        if (nextState != null)
        {
            if (states.ContainsKey(nextState.Name) == false)
                AddState(nextState.Name, nextState);
            ChangeState(nextState.Name);
        }
    }
}
