using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using System;

namespace Unit
{
    [Serializable]
    public class PlayerAnimation : AnimationBase
    {
        public override void Awake()
        {
            base.Awake();
        }

        public override void Start()
        {
            base.Start();
        }

        public void SetMovement(bool isMoving)
        {
            _animator.SetBool(_moveHash, isMoving);
        }

        public void DoAttack()
        {
            _animator.SetTrigger(_attackHash);
        }
    }
}
