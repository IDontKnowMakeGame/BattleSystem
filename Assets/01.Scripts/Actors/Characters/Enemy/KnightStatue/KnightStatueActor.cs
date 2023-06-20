using Acts.Characters;
using Acts.Characters.Enemy;
using Acts.Characters.Enemy.Boss.KnightStatue;
using AI.States;
using Core;
using UnityEngine;

namespace Actors.Characters.Enemy.KnightStatue
{
    public class KnightStatueActor : BossActor
    {
        [SerializeField] private EnemyParticle particle;

        protected override void Init()
        {
            base.Init(); 
            AddAct(_enemyAi);
            AddAct(particle);
            AddAct<CharacterMove>();
            AddAct<KnightStatueAttack>();
        }

        protected override void Start()
        {
            base.Start();
            _enemyAi.CurrentState = _enemyAi.GetState<WaitState>();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<KnightStatueAttack>();
            var jump = _enemyAi.GetState<JumpState>();
            
            jump.OnEnter = () =>
            {
                attack.JumpAttack(1);
            };
        }
    }
}