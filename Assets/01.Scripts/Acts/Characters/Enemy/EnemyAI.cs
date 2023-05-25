using System;
using System.Collections.Generic;
using Actors.Characters;
using Actors.Characters.Enemy;
using Acts.Base;
using AI;
using AI.States;
using Unity.VisualScripting;
using UnityEngine;

namespace Acts.Characters.Enemy
{
    [System.Serializable]
    public class EnemyAI : Act
    {
        public Dictionary<Type, AiState> _states = new();
        public AiState CurrentState;
        private bool _hasEntered = false;
        private bool _hasFinished = false;
        public override void Update()
        {
            
            if(_hasEntered == false)
            {
                CurrentState?.OnEnter?.Invoke();
                if(CurrentState != null)
                    CurrentState.HasPlayed = true;
                _hasEntered = true;
            }
            
            foreach (var currentTransition in CurrentState.Transitions.Values)
            {
                if (currentTransition.CheckCondition())
                {
                    MoveNextState(currentTransition);
                    currentTransition.ResetAllConditions();
                    _hasEntered = false;
                    _hasFinished = true; 
                    CurrentState?.OnExit?.Invoke();
                    return;
                }
            }
            CurrentState?.OnStay?.Invoke();
        }

        public override void Awake()
        {
            SetEnemyAI();
        }

        private void SetEnemyAI()
        {
            var transitions = ThisActor.GetComponents<AiTransition>();
            
            foreach (var transition in transitions)
            {
                var fromType = Type.GetType("AI.States." + transition.From + "State"); 
                var toType = Type.GetType("AI.States." + transition.To + "State");
                AiState instance;
                if (_states.ContainsKey(fromType))
                {
                    instance = _states[fromType];
                }
                else
                {
                    instance = Activator.CreateInstance(fromType) as AiState;
                    _states.Add(fromType, instance);
                    instance.Init();
                }
                transition.SetNextState(toType);
                instance.Transitions.Add(toType, transition);
                //Debug.Log(instance.GetType());
            }
            CurrentState = LoadState(typeof(IdleState));
        }
        
        public T  AddState<T>(T instance = null) where T : AiState
        {
            AiState nextState = null;
            var type = typeof(T);
            if (instance == null)
            {
                nextState = LoadState(type);
            }
            else
            {
                nextState = instance;
                _states.Add(type, nextState);
                nextState.Init();
            }

            return nextState as T;
        }
        public void InitState<T>(T instance = null) where T : AiState
        {
            ResetAllConditions();
            AiState nextState = null;
            var type = typeof(T);
            if (instance == null)
            {
                nextState = LoadState(type);
            }
            else
            {
                nextState = instance;
                _states.Add(type, nextState);
                nextState.Init();
            }
            CurrentState = nextState;
        }
        
        public T GetState<T>() where T : AiState
        {
            var type = typeof(T);
            AiState nextState = LoadState(type);
            return nextState as T;
        }

        public AiState GetState(string stateName)
        {
            var type = Type.GetType("AI.States." + stateName + "State");
            if (_states.ContainsKey(type))
            {
                return _states[type];
            }

            return null;
        }

        public void MoveNextState(AiTransition moveTransition)
        {
            var nextType = moveTransition.NextState;
            AiState nextState = LoadState(nextType);
            CurrentState = nextState;
        }

        private AiState LoadState(Type type)
        {
            AiState nextState = null;
            if (_states.ContainsKey(type))
            {
                nextState = _states[type];
            }
            else
            {
                nextState = (AiState) Activator.CreateInstance(type);
                nextState.Init();
                _states.Add(type, nextState);
            }
            
            return nextState;
        }
        
        public void ResetAllConditions()
        {
            foreach (var currentTransition in CurrentState.Transitions.Values)
            {
                currentTransition.ResetAllConditions();
            }
        }
    }
}