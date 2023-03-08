using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using Behaviour = Units.Behaviours.Default.Behaviour;

namespace Units.Base.Default
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected Vector3 _position;
        [field: SerializeField] public Vector3 SpawnPos { get; set; }
        public Vector3 Position
        {
            get => _position;
            set
            {
                value.y = 0;
                _position = value;
            }
        }

        public int Uuid => GetInstanceID();
        private Dictionary<Type, Behaviour> _behaviours = new();
        
        public T GetBehaviour<T>() where T : Behaviour
        {
            var type = GetBaseType<T>();
            
            if (_behaviours.ContainsKey(type))
            {
                return (T) _behaviours[type];
            }
            return null;
        }
        
        public T AddBehaviour<T>(T instance = default) where T : Behaviour, new()
        {
            var type = GetBaseType<T>();
            
            if (_behaviours.ContainsKey(type))
            {
                return (T) _behaviours[type];
            }

            var behaviour = instance;
            if (behaviour == null)
            {
                behaviour = new T();
            }
            behaviour.ThisBase = this;
            _behaviours.Add(type, behaviour);
            return behaviour;
        }
        
        public T ChangeBehaviour<T>(T instance = default) where T : Behaviour, new()
        {
            var type = GetBaseType<T>();
            var behaviour = instance;
            if (behaviour == null)
            {
                behaviour = new();
            }

            behaviour.ThisBase = this;
            
            if (_behaviours.ContainsKey(type))
            {
                _behaviours[type] = behaviour;
            }
            else
            {
                _behaviours.Add(type, behaviour);
            }
            return behaviour;
        }

        private Type GetBaseType<T>() where T : Behaviour
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;

            while (baseType != typeof(Behaviour))
            {
                thisType = baseType;
                if (thisType != null) baseType = thisType.BaseType;
            }

            return thisType;
        }

        protected virtual void Init()
        {
            Spawn();
            InGame.AddUnit(Uuid, this);
        }

        protected virtual void Awake()
        {
            Init();
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.Awake();
            }   
        }

        protected virtual void Start()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.Start();
            }
        }

        protected virtual void Update()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.Update();
            }
        }

        protected virtual void FixedUpdate()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.FixedUpdate();
            }
        }

        protected virtual void LateUpdate()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.LateUpdate();
            }
        }

        protected virtual void OnEnable()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.OnEnable();
            }
        }

        protected virtual void OnDisable()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.OnDisable();
            }
        }

        protected virtual void OnDestroy()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.OnDestroy();
            }
        }

        protected virtual void OnGUI()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.OnGUI();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            foreach(var behaviour in _behaviours.Values)
            {
                behaviour.OnApplicationQuit();
            }
        }

        public void Spawn()
        {
            transform.position = SpawnPos;
            Position = SpawnPos;
        }
    }
}