using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit
{
    public class AnimationBase : Behaviour
    {
        protected Animator _animator = null;

        // StringToHash
        protected readonly int _moveHash = Animator.StringToHash("Move");
        protected readonly int _attackHash = Animator.StringToHash("Attack");

        public override void Awake()
        {
            _animator = thisBase.GetComponentInChildren<Animator>();
        }
    }
}
