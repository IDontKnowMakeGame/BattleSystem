﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unit.Enemy.AI
{
    public class AIState : Behaviour
    {
        public Unit unit;
        public string Name;
        private List<AITransition> transitions = new List<AITransition>();
        private bool isEntered = false;
        public AIState ExcuteState()
        {
            if (!isEntered)
            {
                OnEnter();
                isEntered = true;
            }
            foreach (var transition in transitions.Where(transition => transition.CheckCondition()))
            {
                isEntered = false;
                OnExit();
                return transition.NextState();
            }

            if (IsEnabled)
            {
                OnStay();
            }
            
            return null;
        }
        
        protected virtual void OnEnter() {}
        protected virtual void OnStay() {}
        protected virtual void OnExit() {}
        
        protected void AddTransition(AITransition transition)
        {
            transitions.Add(transition);
        }
    }
}