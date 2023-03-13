using System;
using Actor.Bases;
using ControllerBase;
using UnityEngine;

namespace Actor.Acts
{
    [RequireComponent(typeof(Controller))]
    public class Act : MonoBehaviour
    {
        protected ActorController _actorController;

        protected virtual void Awake()
        {
            _actorController = GetComponent<ActorController>();
            _actorController.ActList.Add(this.GetType(), this);
        }
    }
}