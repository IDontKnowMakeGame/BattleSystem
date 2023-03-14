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
    [Flags]
    public enum State
    {
        None = 0,
        Move = 1 << 0,
        Attack = 1 << 1,
        Change = 1 << 2,
        Dead = 1 << 3,
    }
    public class ActorController : Controller
    {
        public ItemID WeaponId = 0;

        [SerializeField] private Vector3 _spawnPosition;
        [SerializeField] private State _currentState;
        private ActorStat _actorStat;

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
        public Action<Vector3, Weapon> OnAttack = null;
        public Action OnChange = null;

        public Weapon weapon;
        
        protected virtual void Start()
        {
            Spawn();
            _actorStat = GetAct<ActorStat>();
            Define.GetManager<ItemManager>().weapons
                 .TryGetValue(WeaponId, out weapon);
			weapon.Init(this);
            _actorStat.weaponInfo = weapon.itemInfo;
		}

        private void Spawn()
        {
            var thisTransform = transform;
            thisTransform.position = SpawnPosition;
            Position = thisTransform.position;
            InGame.SetActor(Position, this);
        }
        
        


        public void AddState(State state)
        {
            _currentState |= state;
        }
        
        public void RemoveState(State state)
        {
            _currentState &= ~state;
        }
        
        public bool HasState(State state)
        {
            return (_currentState & state) == state;
        }
    }
}