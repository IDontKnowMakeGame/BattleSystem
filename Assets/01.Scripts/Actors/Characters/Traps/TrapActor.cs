using System;
using Actors.Bases;

namespace Actors.Characters.Traps
{
    public class TrapActor : Actor
    {
        public Func<bool> OnTrapActiveCondition = null;
        public event Action OnTrapTrigger = null;
        public bool IsTrapInput { get; private set; }
        protected override void Update()
        {
            TrapTrigger();
            base.Update();
        }

        public void TrapTrigger()
        {
            if (OnTrapActiveCondition?.Invoke() == true)
            {
                IsTrapInput = true;
                OnTrapTrigger?.Invoke();
            }
            else
            {
                IsTrapInput = false;
            }
        }
    }
}