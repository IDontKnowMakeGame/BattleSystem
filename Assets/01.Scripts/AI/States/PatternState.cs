using System;
using System.Collections.Generic;

namespace AI.States
{
    public class PatternState : AiState
    {
        public List<Action> RandomActions = new();

        public override void Init()
        {
            Name = "Pattern";
            base.Init();
            OnEnter = RandomAction;
        }
        
        private void RandomAction()
        {
            var randomIndex = UnityEngine.Random.Range(0, RandomActions.Count);
            RandomActions[randomIndex]?.Invoke();
        }
    }
}