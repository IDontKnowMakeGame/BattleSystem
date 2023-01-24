using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Behaviours.Unit
{
    public class AnimationBase : UnitBehaviour
    {
        protected Animator animator = null;
        protected readonly int _moveHash = Animator.StringToHash("Move");
        protected readonly int _attackHash = Animator.StringToHash("Attack");

        public override void Awake()
        {
            base.Awake();
            animator = ThisBase.GetComponentInChildren<Animator>();
        }

        public void SetMove(bool moving)
        {
            if(animator != null)
            animator.SetBool(_moveHash, moving);
        }
    }
}
