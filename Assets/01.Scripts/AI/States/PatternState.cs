using System;
using System.Collections.Generic;
using System.Linq;

namespace AI.States
{
    public class NextAction
    {
        public Action Action;
        public float Percent;

        public NextAction(Action action, float f)
        {
            Action = action;
            Percent = f;
        }
    }
    public class PatternState : AiState
    {
        public List<NextAction> RandomActions = new();

        public override void Init()
        {
            Name = "Pattern";
            base.Init();
            OnEnter = RandomAction;
        }
        
        private void RandomAction()
        {
            var randomAction = RandomActions.Where(x => UnityEngine.Random.Range(0, 100) > 100 - x.Percent).OrderBy(x => x.Percent).FirstOrDefault();
            randomAction?.Action?.Invoke();
        }
    }
}