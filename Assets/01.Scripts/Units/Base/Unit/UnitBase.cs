using System;
using System.Collections;
using Core;
using Managements.Managers;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Unit
{
    [Flags]
    public enum BaseState
    {
        None = 1 << 0,
        Moving = 1 << 1,
        Attacking = 1 << 2,
        Skill = 1 << 3,
        Charge = 1 << 4,
        StopMove = 1<< 5,
        Dying = 1 << 6,
        Knockback = 1 << 7,
    }
    public class UnitBase : Units
    {
        [SerializeField] private BaseState state = BaseState.None;
        
        [field:SerializeField] public Vector3 SpawnPos { get; set; }
        public BaseState State
        {
            get => state;
            set => state = value;
        }

        protected override void Init()
        {
            AddBehaviour<UnitRender>();
            base.Init();
        }

        protected override void Start()
        {
            base.Start();
            SpawnSetting();
            InGame.SetUnit(this, Position);
        }

        public void AddState(BaseState state)
        {
            this.state |= state;
        }
        
        public void RemoveState(BaseState state)
        {
            this.state &= ~state;
        }

        public void StartCoroutines(float time, Action after = null, Action before = null) => StartCoroutine(Coroutine(time, after, before));
        public virtual IEnumerator Coroutine(float time, Action after = null, Action before = null)
		{
            before?.Invoke();
            yield return new WaitForSeconds(time);
            after?.Invoke();
        }
        
        
        public void SpawnSetting()
        {
            Position = SpawnPos;
            transform.position = SpawnPos;
        }
    }
}