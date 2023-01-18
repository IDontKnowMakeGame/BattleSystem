﻿using Core;
using Managements.Managers;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;
using Input = Managements.Managers.Input;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        [SerializeField] private PlayerMove playerMove; 
        [SerializeField] private PlayerAttack PlayerAttack;
        [SerializeField] private PlayerEqiq playerEqiq;
        protected override void Init()
        {
            AddBehaviour(thisStat);
            playerMove = AddBehaviour<PlayerMove>();
            PlayerAttack = AddBehaviour<PlayerAttack>();
            playerEqiq = AddBehaviour<PlayerEqiq>();
        }

        protected override void Start()
        {
            Define.GetManager<InputManager>();
            base.Start();
        }
    }
}