using DG.Tweening;
using Units.Base.Unit;
using UnityEngine;
using UnityEngine.InputSystem;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    public class UnitMove : UnitBehaviour
    {
        protected float _duration;
        protected Sequence _seq;
        protected bool isMoving = false;

        public virtual void Translate(InputAction.CallbackContext dir)
        {
            MoveTo(ThisBase.Position + dir.ReadValue<Vector3>());
        }

        public virtual void MoveTo(Vector3 pos, float spd = 1)
        {
            if (ThisBase.State.HasFlag(BaseState.Moving)) return;
            ThisBase.AddState(BaseState.Moving);
            Debug.Log(pos);
        }
    }
}