using System;
using Actor.Bases;
using ControllerBase;
using UnityEngine;

namespace Actor.Acts
{
    [RequireComponent(typeof(Controller))]
    public class Act : MonoBehaviour
    {
        protected Controller _controller;

        protected virtual void Awake()
        {
            _controller = GetComponent<Controller>();
            _controller.ActList.Add(this.GetType(), this);
        }
    }
}