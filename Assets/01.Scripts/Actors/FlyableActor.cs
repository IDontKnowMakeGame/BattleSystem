using System;
using Actors.Bases;
using Core;
using UnityEngine;

namespace Actors
{
    public class FlyableActor : Actor
    {
        private Rigidbody _rigidbody;

        protected override void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            base.Awake();
        }

        protected override void Update()
        {
            UpdatePosition();
            base.Update();
        }

        public void Fly(Vector3 dir, float power)
        {
            if (_rigidbody == null) return;
            _rigidbody.AddForce(dir * power, ForceMode.Force);
        }
        
    }
}