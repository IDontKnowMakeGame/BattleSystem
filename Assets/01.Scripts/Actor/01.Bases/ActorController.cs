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
        public event Action<Weapon> OnChange;

        protected virtual void Awake()
        {
            Define.GetManager<ItemManager>().weapons.TryGetValue(WeaponId, out var weapon);

			Debug.Log(weapon.itemInfo.Ats);
			OnChange?.Invoke(weapon);

			InputManager.OnChangePress += () => OnChange?.Invoke(weapon);
			InputManager.OnMovePress += (pos) => { OnChange?.Invoke(weapon); };
			InputManager.OnMovePress += (pos) => { OnMove?.Invoke(pos, weapon);};
            InputManager.OnMovePress += (pos) => { OnAttack?.Invoke(pos, weapon);};

            Debug.Log(weapon.itemInfo.Afs);
        }

        public T GetAct<T>() where T : Act
        {
            ActList.TryGetValue(typeof(T), out var act);
            return act as T;
        }
    }
}