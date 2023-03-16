using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class AiState
    {
        public string Name;
        public List<AiTransition> Transitions = new();

        public virtual void Init()
        {
            Debug.Log($"Init {Name} State");
        }

        public void SetTarget<T>() where T : AiState
        {
            var transition = new AiTransition();
            transition.SetNextState<T>();
            Transitions.Add(transition);
        }
        public Action OnEnter;
        public Action OnExit;
        public Action OnUpdate;
    }
}