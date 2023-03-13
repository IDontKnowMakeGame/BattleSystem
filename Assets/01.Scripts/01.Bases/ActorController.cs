using System;
using System.Collections.Generic;
using ControllerBase;
using Actor.Acts;
using Core;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Actor.Bases
{
    public class ActorController : Controller
    {
        public ItemID WeaponId = 0;
        [SerializeField] private Vector3 _spawnPosition;

        public Vector3 SpawnPosition
        {
            get => _spawnPosition;
            set
            {
                value.y = 1;
                _spawnPosition = value;
            }
        }

        public Action<Vector3, Weapon> OnMove = null;
        public Action<Vector3, AttackInfo> OnAttack = null;
        public Action OnChange = null;

        public Weapon weapon;
        
        protected virtual void Start()
        {
            Spawn();
            
            Define.GetManager<ItemManager>().weapons.TryGetValue(WeaponId, out weapon);
            Debug.Log(gameObject.name);
            Debug.Log(weapon);
			weapon.Init(this);

			//InputManager.OnChangePress += () => { OnChange?.Invoke(); };
			//InputManager.OnMovePress += (pos) => { OnMove?.Invoke(pos, weapon); };
			//InputManager.OnAttackPress += (pos) => { OnAttack?.Invoke(pos, weapon.AttackInfo); };
		}

        private void Spawn()
        {
            var thisTransform = transform;
            thisTransform.position = SpawnPosition;
            Position = thisTransform.position;
            InGame.SetActor(Position, this);
        }
    }
}