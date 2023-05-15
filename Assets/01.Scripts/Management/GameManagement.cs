using System;
using System.Collections.Generic;
using Actors.Bases;
using Actors.Characters;
using Core;
using DG.Tweening;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Managements
{
    [Serializable]
    public class SpawnCharacter
    {
        public Vector3 Position;
        public GameObject Prefab;
    }
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
        public Dictionary<Type, Manager> Manager => _managers;
        public  List<SpawnCharacter> SpawnCharacters = new();


        #region Control_Managers

        public void AddManager<T>(T instance = null) where T : Manager, new()
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
            if (thisManager == null)
                thisManager = new T();
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

        private void Init()
        {
            instance = this;
            Instance.AddManager<MapManager>();
            Instance.AddManager<DataManager>();
            Instance.AddManager<PoolManager>();
            instance.AddManager<ResourceManager>();
            Instance.AddManager<InputManager<GreatSword>>();
            Instance.AddManager<InputManager<StraightSword>>();
            Instance.AddManager<InputManager<TwinSword>>();
            Instance.AddManager<InputManager<Spear>>();
            Instance.AddManager<InputManager<Bow>>();
            Instance.AddManager<InputManager<Weapon>>();
            Instance.AddManager<ItemManager>();
            Instance.AddManager<EventManager>();
            Define.MainCamera = Camera.main;
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(10,1000);
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
            foreach (var actor in SpawnCharacters)
            {
                var gmObj = GameObject.Instantiate(actor.Prefab);
                gmObj.transform.position = actor.Position;
            }

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
        
        public void MoveScene(string sceneName)
        {
            DOTween.KillAll();
            foreach (var manager in _managers.Values)
            {
                manager.OnDestroy();
            }
            _managers.Clear();
            LoadingSceneController.Instnace.LoadScene(sceneName);
        }

        public void RemoveInputManagers()
        {
            Instance.RemoveManager<InputManager<GreatSword>>();
            Instance.RemoveManager<InputManager<StraightSword>>();
            Instance.RemoveManager<InputManager<TwinSword>>();
            Instance.RemoveManager<InputManager<Spear>>();
            Instance.RemoveManager<InputManager<Bow>>();
            Instance.RemoveManager<InputManager<Weapon>>();
        }
    }
}