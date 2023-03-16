using System;
using System.Collections.Generic;
using Acts.Base;
using AI;
using Unity.VisualScripting;

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
                _hasEntered = true;
            }
            
            foreach (var currentTransition in CurrentState.Transitions)
            {
                if (currentTransition.CheckCondition())
                {
                    MoveNextState(currentTransition);
                    currentTransition.ResetAllConditions();
                    _hasEntered = false;
                    _hasFinished = true;
                    break;
                }
            }
            
            if(_hasFinished == true)
            {
                CurrentState?.OnExit?.Invoke();
                _hasFinished = false;
            }
            else
            {
                CurrentState?.OnUpdate?.Invoke();
            }
                
        }

        public T AddState<T>(T instance = null) where T : AiState
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
        
        public AiState GetState<T>() where T : AiState
        {
            var type = typeof(T);
            AiState nextState = LoadState(type);
            return nextState;
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
    }
}