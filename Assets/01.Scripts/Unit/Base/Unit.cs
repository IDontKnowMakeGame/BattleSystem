using System;
using System.Collections.Generic;
using Manager;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        //The Dictionary that contain the unit's behaviours
        protected Dictionary<Type, Behaviour> behaviours = new Dictionary<Type, Behaviour>();
        
        //Add a behaviour to the unit
        public T AddBehaviour<T>() where T : Behaviour, new()
        {
            var type = typeof(T);
            type = typeof(T).BaseType;
            if (typeof(T).BaseType == typeof(Behaviour))
                type = typeof(T);
            if (behaviours.ContainsKey(type))
            {
                Debug.LogError($"{typeof(T)} is already added to the unit");
                return null;
            }

            var behaviour = new T();
            behaviour.SetBase(this);
            behaviours.Add(type, behaviour);
            return behaviour;
        }
        
        public T AddBehaviour<T>(T behaviour) where T : Behaviour, new()
        {
            var type = typeof(T);
            type = typeof(T).BaseType;
            if (typeof(T).BaseType == typeof(Behaviour))
                type = typeof(T);
            if (behaviours.ContainsKey(type))
            {
                Debug.LogError($"{typeof(T)} is already added to the unit");
                return null;
            }

            behaviour.SetBase(this);
            behaviours.Add(type, behaviour);
            return behaviour;
        }
        
        //Get a behaviour from the unit
        public T GetBehaviour<T>() where T : Behaviour
        {
            var type = typeof(T);
            type = typeof(T).BaseType;
            if (typeof(T).BaseType == typeof(Behaviour))
                type = typeof(T);
            return (T)behaviours[type];
        }
        
        //Remove a behaviour from the unit
        public void RemoveBehaviour<T>() where T : Behaviour
        {
            var type = typeof(T);
            type = typeof(T).BaseType;
            if (typeof(T).BaseType == typeof(Behaviour))
                type = typeof(T);
            behaviours.Remove(type);
        }
    }
}