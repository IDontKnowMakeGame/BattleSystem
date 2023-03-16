using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class AiTransition
    {
        public AiConditionHolder ConditionHolder;
        private List<AiCondition> _conditions = new();
        private Type _nextState;
        public Type NextState  => _nextState;

        public bool CheckCondition()
        {
            bool isSatisfied = true;
            var needCondition = _conditions.FindAll((condition) => condition.IsNeeded);
            var unNeedCondition = _conditions.FindAll((condition) => !condition.IsNeeded);
            
            foreach (var currentCondition in unNeedCondition)
            {
                isSatisfied |= currentCondition.IsSatisfied() != currentCondition.IsNegative;
            }
            foreach (var currentCondition in needCondition)
            {
                isSatisfied &= currentCondition.IsSatisfied() != currentCondition.IsNegative;
            }

            return isSatisfied;
        }
        
        public void SetNextState<T>() where T : AiState
        {
            _nextState = typeof(T);
        }

        public void Init()
        {
            foreach (var condition in ConditionHolder.Conditions)
            {
                var type = Type.GetType("AI.Conditions." + condition.Type);
                var newCondition = (AiCondition) Activator.CreateInstance(type);
                newCondition.IsNegative = condition.IsNegative;
                newCondition.IsNeeded = condition.IsNeeded;
                newCondition.stringParam = condition.stringParam;
                newCondition.floatParam = condition.floatParam;
                newCondition.intParam = condition.intParam;
                newCondition.actorParam = condition.actorParam;
                newCondition.vectorParam = condition.vectorParam;
                _conditions.Add(newCondition);
            }
        }

        public void ResetAllConditions()
        {
            foreach (var condition in _conditions)
            {
                condition.Reset();
            }
        }
    }
}