using System;
using System.Collections.Generic;
using Actor.Acts;
using Managements.Managers;
using UnityEngine;

namespace Actor.Bases
{
    public class ActorController : MonoBehaviour
    {
        public Dictionary<Type, Act> ActList = new ();
        public event Action<Vector3> OnMove;

        protected virtual void Awake()
        {
            InputManager.OnMovePressed += OnMove;
        }

        public T GetAct<T>() where T : Act
        {
            ActList.TryGetValue(typeof(T), out var act);
            return act as T;
        }
    }
}