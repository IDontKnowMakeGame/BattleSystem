using System;
using System.ComponentModel.Design.Serialization;
using Actors.Bases;
using Acts.Characters;
using Core;
using Data;
using UnityEngine;

namespace Actors.Characters
{
    [Flags]
    public enum CharacterState
    {
        None = 0,
        Move = 1 << 0,
        Attack = 1 << 1,
        Skill = 1 << 2,
        Hold = 1 << 3,
        StopMove = 1 << 4,              
        Stun = 1 << 5,
        Equip = 1 << 6,
        NuckBack = 1<<7, 
        Everything = ~None,
    }
    public class CharacterActor : Actor
    {
        [SerializeField] private CharacterRender _characterRender;
        [SerializeField] private CharacterState _characterState;

        public Weapon currentWeapon;
		protected override void Init()
        {
            base.Init();
            AddAct(_characterRender);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            InGame.SetActorOnBlock(this, Position);
            base.Start();
        }
        
        public void AddState(CharacterState state)
        {
            _characterState |= state;
        }
        
        public void RemoveState(CharacterState state)
        {
            _characterState &= ~state;
        }
        
        public bool HasState(CharacterState state)
        {
            return (_characterState & state) != CharacterState.None;
        }

        public bool HasAnyState()
        {
            return (_characterState & CharacterState.Everything) != CharacterState.None;
        }    
    }
}