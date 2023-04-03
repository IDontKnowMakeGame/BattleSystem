using System;
using Acts.Characters.Enemy;
using AI;
using UnityEngine;

namespace Actors.Characters.Enemy
{
    public class EnemyActor : CharacterActor
    {
        [SerializeField] protected float _secondPhaseHpPercent;
        [SerializeField] protected EnemyAI _enemyAi;
        [SerializeField] protected CharacterEquipmentAct _characterEquipment;
		[SerializeField] private EnemyStatAct _characterStat;
        [SerializeField] private EnemyAnimation _enemyAnimation;

		protected override void Init()
        {
            AddAct(_characterEquipment);
            AddAct(_characterStat);
            AddAct(_enemyAnimation);
            base.Init();
        }

        protected bool IsSecondPhase()
        {
            var stat = GetAct<CharacterStatAct>();
            var maxHp = stat.BaseStat.hp;
            var hp = stat.ChangeStat.hp;
            var result = (hp / maxHp) * 100f < _secondPhaseHpPercent;
            return result;
        }
    }
}