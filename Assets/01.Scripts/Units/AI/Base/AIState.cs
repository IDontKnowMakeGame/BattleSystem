using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Unit.Base.AI
{
    public class AIState : Behaviour
    {
        public Units.Base.Units unit;
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