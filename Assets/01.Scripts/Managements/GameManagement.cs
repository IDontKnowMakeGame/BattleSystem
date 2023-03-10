using System;
using System.Collections.Generic;
using Managements.Managers;
using Managements.Managers.Base;
using Managements.Managers.Floor;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managements
{
    public class GameManagement : MonoBehaviour
    {
        private static GameManagement instance = null;

        public static GameManagement Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameManagement>();
                return instance;
            }
        }

        private Dictionary<Type, Manager> _managers = new();


        #region Control_Managers

        public T AddManager<T>() where T : Manager, new()
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Manager))
            {
                thisType = baseType;
            }

            if (_managers.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return null;
            }

            var thisManager = new T();
            thisManager.Instance = this;
            _managers.Add(thisType, thisManager);
            return thisManager;
        }

        public void AddManager<T>(T instance) where T : Manager
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Manager))
            {
                thisType = baseType;
            }

            if (_managers.ContainsKey(thisType))
            {
                Debug.LogError($"{thisType} is already in this Unit.");
                return;
            }

            var thisManager = instance;
            thisManager.Instance = this;
            _managers.Add(thisType, thisManager);
        }

        public void UpdateManager<T>(T instance) where T : Manager
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Manager))
            {
                thisType = baseType;
            }

            if (_managers.ContainsKey(thisType))
            {
                _managers[thisType] = instance;
                _managers[thisType].Instance = this;
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
        }

        public void RemoveManager<T>() where T : Manager
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Manager))
            {
                thisType = baseType;
            }

            if (_managers.ContainsKey(thisType))
            {
                _managers.Remove(thisType);
            }
            else
            {
                Debug.LogError($"This unit doesn't have {thisType}.");
            }
        }

        public T GetManager<T>() where T : Manager
        {
            var thisType = typeof(T);
            var baseType = typeof(T).BaseType;
            if (baseType != typeof(Manager))
            {
                thisType = baseType;
            }

            if (_managers.ContainsKey(thisType))
            {
                return (T)_managers[thisType];
            }
            else
            {
                //Debug.LogError($"This unit doesn't have {thisType}.");
                return null;
            }
        }
        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void FirstStart()
        {
            instance = GameManagement.Instance;
        }

        private void Init()
        {
            instance = this;
            Instance.AddManager<InputManager>();
            Instance.AddManager<DataManager>();
            Instance.AddManager<ResourceManagers>();
            Instance.AddManager<PoolManager>();
            Instance.AddManager<EventManager>();
            Instance.AddManager<MapManager>();
            Instance.AddManager<UIManager>();
            Instance.AddManager<FloorManager>();
        }

        public void Awake()
        {
            Init();
            foreach (var manager in _managers.Values)
            {
                manager.Awake();
            }
        }

        public void Start()
        {
            foreach (var manager in _managers.Values)
            {
                manager.Start();
            }
        }

        public void Update()
        {
            foreach (var manager in _managers.Values)
            {
                manager.Update();
            }
        }

        public void FixedUpdate()
        {
            foreach (var manager in _managers.Values)
            {
                manager.FixedUpdate();
            }
        }

        public void LateUpdate()
        {
            foreach (var manager in _managers.Values)
            {
                manager.LateUpdate();
            }
        }

        public void OnEnable()
        {
            foreach (var manager in _managers.Values)
            {
                manager.OnEnable();
            }
        }

        public void OnDisable()
        {
            foreach (var manager in _managers.Values)
            {
                manager.OnDisable();
            }
        }

        public void OnDestroy()
        {
            foreach (var manager in _managers.Values)
            {
                manager.OnDestroy();
            }
        }
    }
}
