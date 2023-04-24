using Actors.Bases;
using Core;
using UnityEngine;

namespace AI.Conditions
{
    [System.Serializable]
    public class TargetCondition : AiCondition
    {
        [SerializeField] private Actor targetActor;

        public Actor TargetActor => IsPlayer ? InGame.Player : targetActor;
        public bool IsPlayer = true;
        
        
    }
}