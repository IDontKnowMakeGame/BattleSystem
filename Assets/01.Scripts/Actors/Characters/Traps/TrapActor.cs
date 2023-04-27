using System;
using Actors.Bases;

namespace Actors.Characters.Traps
{
    public class TrapActor : CharacterActor
    {
        public Func<bool> OnTrapActiveCondition = null;
        public event Action OnTrapTrigger = null;
        public bool IsTrapInput { get; private set; }
        protected override void Update()
        {
            TrapTrigger();
            base.Update();
        }

        protected override void Init()
        {
            IsUpdatingPosition = false;
        }

        protected override void Start()
        {
            base.Start();
        }

        public void TrapTrigger()
        {
            if (OnTrapActiveCondition?.Invoke() == true)
            {
                if(IsTrapInput == false)
                    OnTrapTrigger?.Invoke();
                IsTrapInput = true;
            }
            else
            {
                IsTrapInput = false;
            }
        }
    }
}