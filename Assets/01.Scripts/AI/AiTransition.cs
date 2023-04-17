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
        [ContextMenuItem("Add Block Condition", "AddBlockCondition")]
        [ContextMenuItem("Add Line Condition", "AddLineCondition")]
        [ContextMenuItem("Add Circle Condition", "AddCircleCondition")]
        [ContextMenuItem("Add Move Condition", "AddMoveCondition")]
        [SerializeReference]
        public List<AiCondition> _conditions = new();
        private Type _nextState;
        public Type NextState  => _nextState;

        public bool CheckCondition()
        {
            bool isSatisfied = false;
            var needCondition = _conditions.FindAll((condition) => condition.IsNeeded);
            var unNeedCondition = _conditions.FindAll((condition) => !condition.IsNeeded);
            
            foreach (var currentCondition in unNeedCondition)
            {
                isSatisfied |= currentCondition.IsSatisfied() != currentCondition.IsNegative;
            }

            if (unNeedCondition.Count == 0)
                isSatisfied = true;
            
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
        
        public void AddBlockCondition()
        {
            var condition = new BlockCondition();
            condition.Type = Condition.BlockCondition;
            _conditions.Add(condition);
        }
        
        public void AddLineCondition()
        {
            var condition = new LineCondition();
            condition.Type = Condition.LineCondition;
            _conditions.Add(condition);
        }
        
        public void AddCircleCondition()
        {
            var condition = new CircleCondition();
            condition.Type = Condition.CircleCondition;
            _conditions.Add(condition);
        }
        
        public void AddMoveCondition()
        {
            var condition = new MoveCondition();
            condition.Type = Condition.MoveCondition;
            _conditions.Add(condition);
        }
    }
}