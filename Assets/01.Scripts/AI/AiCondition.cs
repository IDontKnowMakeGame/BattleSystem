using Actors.Bases;
using UnityEngine;

namespace AI
{
    public enum Condition
    {
        TimeCondition,
        DistanceCondition,
        BesideCondition,
        AttackCondition,
        LifeCondition,
        BlockCondition,
        LineCondition,
        CircleCondition,
        MoveCondition,
        EdgeCondition,
        EnteredCondition,
        MovableCondition,
        StateCondition,
    }
    
    [System.Serializable]
    public class AiCondition
    {
        public Condition Type;

        public virtual bool IsSatisfied() => true;
        public bool IsNegative;
        public bool IsNeeded;
        public Actor _thisActor;

        public virtual void Reset()
        {
            
        }
    }
}