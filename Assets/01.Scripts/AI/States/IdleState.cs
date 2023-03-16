using AI.Conditions;
using UnityEngine;

namespace AI.States
{
    public class IdleState : AiState
    {
        TimeCondition _timeCondition;
        public override void Init()
        {
            Name = "Idle";
            base.Init();
        }
    }
}