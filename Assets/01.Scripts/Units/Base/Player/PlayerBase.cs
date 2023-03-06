using System;
using Core;
using Managements.Managers;
using Unit.Block;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Units.Base.Player
{
    public class PlayerBase : UnitBase
    {
        [SerializeField] private PlayerMove playerMove;
        [SerializeField] private PlayerKnockback playerKnockback;
        [SerializeField] private PlayerAttack PlayerAttack;
        [SerializeField] private PlayerEquiq PlayerEqiq;
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private PlayerBuff playerBuff;
        [SerializeField] private PlayerStat thisStat;
        [SerializeField] private PlayerItem playerItem;
        [SerializeField] private CharacterRender characterRender;
        [SerializeField] private UnitCollider unitCollider;

        protected override void Init()
        {
            InGame.PlayerBase = this;
            AddBehaviour(thisStat);
            AddBehaviour(playerAnimation);
            AddBehaviour(playerMove);
            AddBehaviour(playerKnockback);
            AddBehaviour(PlayerAttack);
            AddBehaviour(playerBuff);
            AddBehaviour(PlayerEqiq);
            AddBehaviour(playerItem);
            AddBehaviour(unitCollider);
            base.Init();
            ChangeBehaviour(characterRender);
        }

        protected override void Start()
        {
            Define.GetManager<InputManager>();
            base.Start();
        }

        protected override void Update()
        {
            var block = Define.GetManager<MapManager>().GetBlock(Position);
            block.GetBehaviour<BlockRender>().SetOutlineColor(Color.white); 
            base.Update();
        }
    }
}