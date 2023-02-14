using System;
using System.Collections.Generic;
using Units.Behaviours.Unit;
using UnityEngine;
using Behaviour = Units.Behaviours.Base.Behaviour;

namespace Units.Base
{
    public class Units : MonoBehaviour
    {
        private Dictionary<Type, Behaviour> _behaviours = new();
        [SerializeField] private Vector3 position = Vector3.zero;
        public Vector3 Position
        {
            get => position;
            set
            {
                value.y = 0;
                position = value;
            }
        }

        #region Unit_LifeCycle

        protected virtual void Init()
        {
            //Add or Init Behaviours
            Position = transform.position;
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

        protected virtual void OnApplicationQuit()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.OnApplicationQuit();
            }
        }

        #endregion

        #region Control_Behaviours
        public T AddBehaviour<T>() where T : Behaviour, new()
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return null;
            }
            
            var thisBehaviour = new T();
            thisBehaviour.ThisBase = this;
            _behaviours.Add(thisType, thisBehaviour);
            return thisBehaviour;
        }

        public void AddBehaviour<T>(T instance) where T : Behaviour
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return;
            }
            
            var thisBehaviour = instance;
            thisBehaviour.ThisBase = this;
            _behaviours.Add(thisType, thisBehaviour);
        }

        public void UpdateBehaviour<T>(T instance) where T : Behaviour
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                _behaviours[thisType] = instance;
                _behaviours[thisType].ThisBase = this;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }   
        }

        public void RemoveBehaviour<T>() where T : Behaviour
        {
            var thisType = GetBaseType<T>();

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
            var thisType = GetBaseType<T>();

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
        
        public void ChangeBehaviour<T>(T instance) where T : Behaviour
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                _behaviours[thisType] = instance;
                _behaviours[thisType].ThisBase = this;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
        }
        
        public T ChangeBehaviour<T> () where T : Behaviour, new()
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                var thisBehaviour = new T();
                thisBehaviour.ThisBase = this;
                _behaviours[thisType] = thisBehaviour;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
            return _behaviours[thisType] as T;
        }

        private Type GetBaseType<T>() where T : Behaviour
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;

            while (baseType != typeof(Behaviour) && baseType != typeof(UnitBehaviour))
            {
                thisType = baseType;
                if (thisType != null) baseType = thisType.BaseType;
            }

            return thisType;
        }


        protected void LogBehaviours()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                Debug.Log(behaviour);
            }
        }
        
        #endregion
    }
}