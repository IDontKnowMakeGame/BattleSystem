using System;
using System.Collections.Generic;

namespace AI.States
{
    public class RandomState : AiState
    {
        public List<Action> RandomList = new();
        public override void Init()
        {
            Name = "Random";
            base.Init();
            OnEnter += RandomAction;
        }
        
        private void RandomAction()
        {
            var random = UnityEngine.Random.Range(0, RandomList.Count);
            RandomList[random]?.Invoke();
        }
    }
}