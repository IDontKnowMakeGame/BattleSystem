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
    }
    [System.Serializable]
    public class AiCondition
    {
        public Condition Type;

        public virtual bool IsSatisfied() => true;
        public bool IsNegative;
        public bool IsNeeded;
        public Actor _thisActor;

        public string stringParam;
        public float floatParam;
        public int intParam; 
        public Actor actorParam;
        public Vector3 vectorParam;


        public virtual void Reset()
        {
            
        }
    }
}