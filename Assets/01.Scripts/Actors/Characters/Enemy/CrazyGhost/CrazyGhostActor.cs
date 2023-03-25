using Acts.Characters;
using Acts.Characters.Enemy;
using AI;
using AI.Conditions;
using AI.States;
using Core;
using UnityEngine;

namespace Actors.Characters.Enemy.CrazyGhost
{
    public class CrazyGhostActor : EnemyActor
    {
        protected override void Init()
        {
            base.Init(); 
            AddAct(_enemyAi);
            AddAct<CharacterMove>();
            AddAct<EnemyAttack>();
        }

        protected override void Start()
        {
            base.Start();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<EnemyAttack>();
            var chase = _enemyAi.GetState<ChaseState>();
            chase.OnStay += () =>
            {
                move.Chase(InGame.Player);
            };
            
            var random = _enemyAi.GetState<RandomState>();

            random.RandomList.Add(() => 
            {
                var dir = InGame.Player.Position - Position;
                attack.ForwardAttack(dir, true);
            });
            random.RandomList.Add(() =>
            {
                var dir = InGame.Player.Position - Position;
                attack.BackAttack(dir, true);
            });
            random.RandomList.Add(() =>
            {
                var dir = InGame.Player.Position - Position;
                attack.TripleAttack(dir, true);
            });
        }
    }
}