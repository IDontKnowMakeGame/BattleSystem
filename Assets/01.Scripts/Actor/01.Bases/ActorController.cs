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
        public event Action<Vector3, Weapon> OnAttack;
        public event Action OnChange;


        private Weapon _weapon;
        public Weapon weapon => _weapon;
        protected virtual void Start()
        {
            Define.GetManager<ItemManager>().weapons.TryGetValue(WeaponId, out _weapon);

			InputManager.OnChangePress += () => { OnChange?.Invoke(); };
			InputManager.OnMovePress += (pos) => { OnMove?.Invoke(pos, weapon);};
            InputManager.OnMovePress += (pos) => { OnAttack?.Invoke(pos, weapon);};
        }

        public T GetAct<T>() where T : Act
        {
            ActList.TryGetValue(typeof(T), out var act);
            return act as T;
        }
    }
}