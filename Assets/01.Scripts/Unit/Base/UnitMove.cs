using System;
using UnityEngine;

namespace Unit
{
    public class UnitMove : Behaviour
    {
        public Vector3 position;

        public override void Start()
        {
            position = thisBase.transform.position;
        }

        public virtual void Translate(Vector3 dir)
        {

        }
    }
}