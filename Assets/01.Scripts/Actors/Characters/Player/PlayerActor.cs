﻿using Acts.Characters.Player;
using Core;
using UnityEngine;

namespace Actors.Characters.Player
{

    public class PlayerActor : CharacterActor
    {
        [SerializeField] private PlayerMove _playerMove;
		[SerializeField] private PlayerStatAct _playerStat;
        [SerializeField] private PlayerEquipment _playerEquipment;
        [SerializeField] private PlayerAnimation _plyerAnimation;
        [SerializeField] private PlayerAttack _playerAttack;
        [SerializeField] private PlayerUseAbleItem _playerUseAbleItem;
        [SerializeField] private PlayerBuff _playerBuff;

		protected override void Init()
        { 
            base.Init();
            AddAct(_playerStat);
            AddAct(_plyerAnimation);
			AddAct(_playerEquipment);
            AddAct(_playerMove);
            AddAct(_playerAttack);
            AddAct(_playerUseAbleItem);
            AddAct(_playerBuff);
            var detect = AddAct<PlayerDetect>();
            InGame.Player = this;
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                var actors = InGame.GetNearCharacterActors(Position);
                foreach (var actor in actors)
                {
                    if(actor == null) Debug.Log(null);
                    else Debug.Log(actor.name);
                }
            }
            base.Update();
        }
    }
}