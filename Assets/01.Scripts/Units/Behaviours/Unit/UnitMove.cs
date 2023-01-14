using DG.Tweening;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Behaviours.Unit
{
    public class UnitMove : Behaviour
    {
        private float _duration;
        private Sequence _seq;

        public void Translate(Vector3 dir)
        {
            MoveTo(thisBase.Position + dir);
        }

        public void MoveTo(Vector3 pos)
        {
               
        }
    }
}