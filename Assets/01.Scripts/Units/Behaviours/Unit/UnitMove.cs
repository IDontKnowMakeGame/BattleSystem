using DG.Tweening;
using Units.Base.Unit;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    public class UnitMove : UnitBehaviour
    {
        protected float _duration;
        protected Sequence _seq;
        protected bool isMoving = false;

        public float distance = 1;
        public virtual void Translate(Vector3 dir, float spd = 1)
        {
            MoveTo(ThisBase.Position + dir * distance, spd);
        }

        public virtual void MoveTo(Vector3 pos, float spd = 1)
        {
            if (ThisBase.State.HasFlag(BaseState.Moving) || ThisBase.State.HasFlag(BaseState.StopMove)) return;
            ThisBase.AddState(BaseState.Moving);
        }
    }
}