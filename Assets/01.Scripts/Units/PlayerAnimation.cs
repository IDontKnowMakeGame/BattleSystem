using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerAnimation : AnimationBase
    {
        public void AttackAnimation()
        {
            animator.SetTrigger(_attackHash);
        }
    }
}
