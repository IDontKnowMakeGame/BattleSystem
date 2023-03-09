using System;
using Actor.Bases;
using UnityEngine;

namespace Actor.Acts
{
    [RequireComponent(typeof(ActorController))]
    public class Act : MonoBehaviour
    {
        private ActorController _actorController;

        protected virtual void Awake()
        {
            _actorController = GetComponent<ActorController>();
            _actorController.ActList.Add(this.GetType(), this);
        }
    }
}