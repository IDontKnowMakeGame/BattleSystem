using System;
using System.Collections.Generic;
using Actors.Characters.Enemy;
using AI.Conditions;
using UnityEditor;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class AiTransition : MonoBehaviour
    {
        public string From;
        public string To;
        [ContextMenuItem("Add Time Condition", "AddTimeCondition")]
        [ContextMenuItem("Add Beside Condition", "AddBesideCondition")]
        [ContextMenuItem("Add Attack Condition", "AddAttackCondition")]
        [ContextMenuItem("Add Life Condition", "AddLifeCondition")]
        [SerializeReference]
        public List<AiCondition> _conditions = new();
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
        
        public void SetNextState(Type type)
        {
            _nextState = type;
        }

        public void ResetAllConditions()
        {
            foreach (var condition in _conditions)
            {
                condition.Reset();
            }
        }
        
        public void AddTimeCondition()
        {
            var condition = new TimeCondition();
            condition.Type = Condition.TimeCondition;
            _conditions.Add(condition);
        }
        
        public void AddBesideCondition()
        {
            var condition = new BesideCondition();
            condition.Type = Condition.BesideCondition;
            _conditions.Add(condition);
        }
        
        public void AddAttackCondition()
        {
            var condition = new AttackCondition();
            condition.Type = Condition.AttackCondition;
            _conditions.Add(condition);
        }
        
        public void AddLifeCondition()
        {
            var condition = new LifeCondition();
            condition.Type = Condition.LifeCondition;
            _conditions.Add(condition);
        }
    }
}