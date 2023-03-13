using System;
using System.Collections.Generic;
using Actor.Acts;
using UnityEngine;

namespace ControllerBase
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Vector3 _position;

        public Vector3 Position
        {
            get => _position;
            set
            {
                value.y = 0;
                _position = value;
            }
        }
        
        public Dictionary<Type, Act> ActList = new ();

        public T GetAct<T>() where T : Act
        {
            ActList.TryGetValue(typeof(T), out var act);
            return act as T;
        }
    }
}