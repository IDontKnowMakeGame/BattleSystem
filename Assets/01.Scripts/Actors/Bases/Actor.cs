using UnityEngine;
using System;
using System.Collections.Generic;
using Acts.Base;
using Acts.Characters;
using Core;
using Managements.Managers;
using UnityEngine.SocialPlatforms.Impl;

namespace Actors.Bases
{
    public class Actor : MonoBehaviour
    {
        public int UUID => gameObject.GetInstanceID();
        private Dictionary<Type, Act> _behaviours = new();
        public Action<float> OnDirectionUpdate = null;
        [SerializeField] private Vector3 position = Vector3.zero;
        protected Transform spriteTransform;
        public bool IsUpdatingPosition { get; protected set; } = true;

        public Vector3 Position => position;

        public Transform SpriteTransform => spriteTransform;
        
        #region Unit_LifeCycle

        protected virtual void Init()
        {
            //Add or Init Acts
            spriteTransform = this.GetComponentInChildren<MeshRenderer>().transform;
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
            InGame.AddActor(this);
            foreach (var behaviour in _behaviours.Values)
            {
                behaviour.Start();
            }
        }

        protected virtual void Update()
        {
            if(IsUpdatingPosition)
                UpdatePosition();
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

        #region Control_Acts

        public T AddAct<T>(T instance = null) where T : Act, new()
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return _behaviours[thisType] as T;
            }

            var thisAct = instance;
            if(thisAct == null)
                thisAct = new T();
            thisAct.ThisActor = this;
            _behaviours.Add(thisType, thisAct);
            return thisAct;
        }

        public void UpdateAct<T>(T instance) where T : Act
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                _behaviours[thisType] = instance;
                _behaviours[thisType].ThisActor = this;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
        }

        public void RemoveAct<T>() where T : Act
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

        public T GetAct<T>() where T : Act
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                return (T)_behaviours[thisType];
            }
            else
            {
                //Debug.LogError($"This unit doesn't have {thisType}.");
                return null;
            }
        }

        public void ChangeAct<T>(T instance) where T : Act
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                _behaviours[thisType] = instance;
                _behaviours[thisType].ThisActor = this;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
        }

        public T ChangeAct<T>() where T : Act, new()
        {
            var thisType = GetBaseType<T>();

            if (_behaviours.ContainsKey(thisType))
            {
                var thisAct = new T();
                thisAct.ThisActor = this;
                _behaviours[thisType] = thisAct;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }

            return _behaviours[thisType] as T;
        }

        private Type GetBaseType<T>() where T : Act
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;

            while (baseType != typeof(Act))
            {
                thisType = baseType;
                if (thisType != null) baseType = thisType.BaseType;
            }

            return thisType;
        }


        protected void LogActs()
        {
            foreach (var behaviour in _behaviours.Values)
            {
                Debug.Log(behaviour);
            }
        }

        #endregion

        protected virtual void UpdatePosition()
        {
            var pos = transform.position;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = 0;
            pos.z = Mathf.RoundToInt(pos.z);
            position = pos;
        }
    }
}