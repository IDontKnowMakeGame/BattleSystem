using System;
using System.Collections.Generic;
using Actor.Acts;
using Core;
using Managements.Managers;
using UnityEngine;

namespace Actor.Bases
{
    public class ActorController : MonoBehaviour
    {
        public ItemID WeaponId = 0;
        public Dictionary<Type, Act> ActList = new ();
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
        public event Action<Vector3, Weapon> OnMove;

        protected virtual void Awake()
        {
            Define.GetManager<ItemManager>().weapons.TryGetValue(WeaponId, out var weapon);
            InputManager.OnMovePressed += (pos) => { OnMove?.Invoke(pos, weapon);};
        }

        public T GetAct<T>() where T : Act
        {
            ActList.TryGetValue(typeof(T), out var act);
            return act as T;
        }
    }
}