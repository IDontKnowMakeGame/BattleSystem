﻿using Acts.Characters.Enemy;
using UnityEngine;

namespace Actors.Characters.Enemy
{
    public class EnemyActor : CharacterActor
    {
        [SerializeField] protected EnemyAI _enemyAi;
        [SerializeField] protected CharacterEquipmentAct _characterEquipment;

        protected override void Init()
        {
            AddAct(_characterEquipment);
            base.Init();
        }
    }
}