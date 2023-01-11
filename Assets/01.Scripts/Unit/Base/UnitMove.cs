﻿using System;
using UnityEngine;

namespace Unit
{
    public class UnitMove : Behaviour
    {
        public Vector3 position;
        protected bool isMoving = false;
        public override void Start()
        {
            position = thisBase.transform.position;
        }

        public virtual void Translate(Vector3 dir, float speed = 0)
        {

        }
        public bool IsMoving() => isMoving;
    }
}