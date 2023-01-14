using System;
using System.Collections.Generic;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Base
{
    public class Units : MonoBehaviour
    {
        private Dictionary<Type, Behaviour> _behaviours = new();
        public Vector3 Position { get; set; }

        #region Unit_LifeCycle

        protected virtual void Init()
        {
            //Add or Init Behaviours
        }

        protected virtual void Awake()
        {
            Init();
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.Awake();
            }
        }

        protected virtual void Start()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.Start();
            }
        }

        protected virtual void Update()
        {   
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.Update();
            }
        }

        protected virtual void FixedUpdate()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.FixedUpdate();
            }
        }

        protected virtual void LateUpdate()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.LateUpdate();
            }
        }

        protected virtual void OnEnable()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.OnEnable();
            }
        }

        protected virtual void OnDisable()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.OnDisable();
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.OnDestroy();
            }
        }

        #endregion
        
        #region Control_Behaviours
        public T AddBehaviour<T>() where T : Behaviour, new()
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Behaviour))
            {
                thisType = baseType;
            }

            if (_behaviours.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return null;
            }
            
            var thisBehaviour = new T();
            thisBehaviour.thisBase = this;
            _behaviours.Add(thisType, thisBehaviour);
            return thisBehaviour;
        }

        public void AddBehaviour<T>(T instance) where T : Behaviour
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Behaviour))
            {
                thisType = baseType;
            }
            
            if (_behaviours.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return;
            }
            
            var thisBehaviour = instance;
            thisBehaviour.thisBase = this;
            _behaviours.Add(thisType, thisBehaviour);
        }

        public void UpdateBehaviour<T>(T instance) where T : Behaviour
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Behaviour))
            {
                thisType = baseType;
            }

            if (_behaviours.ContainsKey(thisType))
            {
                _behaviours[thisType] = instance;
                _behaviours[thisType].thisBase = this;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }   
        }

        public void RemoveBehaviour<T>() where T : Behaviour
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Behaviour))
            {
                thisType = baseType;
            }

            if (_behaviours.ContainsKey(thisType))
            {
                _behaviours.Remove(thisType);
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
        }
        
        public T GetBehaviour<T>() where T : Behaviour
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Behaviour))
            {
                thisType = baseType;
            }

            if (_behaviours.ContainsKey(thisType))
            {
                return (T) _behaviours[thisType];
            }
            else
            {
                //Debug.LogError($"This unit doesn't have {thisType}.");
                return null;
            }
        }
        #endregion
        
    }
}