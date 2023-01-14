using DG.Tweening;
using Units.Base.Unit;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    public class UnitMove : UnitBehaviour
    {
        private float _duration;
        private Sequence _seq;

        public virtual void Translate(Vector3 dir)
        {
            MoveTo(ThisBase.Position + dir);
        }

        public virtual void MoveTo(Vector3 pos)
        {
            if (ThisBase.State.HasFlag(BaseState.Moving)) return;
            ThisBase.AddState(BaseState.Moving);
            Debug.Log(pos);
        }
    }
}