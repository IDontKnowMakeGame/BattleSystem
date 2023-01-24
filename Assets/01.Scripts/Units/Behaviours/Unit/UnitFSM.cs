using System;
using System.Collections;
using System.Collections.Generic;
using Unit.Base.AI;
using UnityEditor.Rendering;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

public class UnitFSM : Behaviour
{
    private AIState currentState;
    private AIState nextState = null;
    private readonly Dictionary<Type, AIState> states = new();
    
    
    public bool ChangeState(Type state)
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

    private AIState AddState(Type state, AIState aiState)
    {
        if (states.ContainsKey(state) == false)
        {
            aiState.ThisBase = ThisBase;
            aiState.Awake();
            states.Add(state, aiState);
        }

        return aiState;
    }

    public void SetDefaultState<T>() where T : AIState, new()
    {
        var roamingState = new T();
        currentState = AddState(typeof(T), roamingState);
    }

    public override void Update()
    {
        nextState = currentState.ExcuteState();
        if (nextState != null)
        {
            if (states.ContainsKey(nextState.GetType()) == false)
                AddState(nextState.GetType(), nextState);
            ChangeState(nextState.GetType());
        }
    }
}
