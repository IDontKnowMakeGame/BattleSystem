using System;
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
        Dying = 1 << 3,
    }
    public class UnitBase : Units
    {
        [SerializeField] private BaseState state = BaseState.None;
        [SerializeField] protected UnitStat thisStat = null;
        
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
    }
}