using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class AiState
    {
        public string Name;
        public Dictionary<Type, AiTransition> Transitions = new();
        public bool HasPlayed { get; set; }

        public virtual void Init()
        {
            //Debug.Log($"Init {Name} State");
        }
        
        public Action OnEnter;
        public Action OnExit;
        public Action OnStay;
    }
}