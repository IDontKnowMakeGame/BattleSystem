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
            var move = AddAct<CharacterMove>();
            var attack = AddAct<EnemyAttack>();
            
            var idle = _enemyAi.AddState<IdleState>();
            idle.SetTarget<ChaseState>();
            
            var chase = _enemyAi.AddState<ChaseState>();
            chase.OnStay += () =>
            {
                move.Chase(InGame.Player);
            };
            chase.SetTarget<RandomState>();
            
            
            var random = _enemyAi.AddState<RandomState>();
            // random.RandomList.Add(() =>
            // {
            //     var dir = InGame.Player.Position - Position;
            //     attack.ForwardAttak(dir, true);
            // });
            // random.RandomList.Add(() =>
            // {
            //     var dir = InGame.Player.Position - Position;
            //     attack.BackAttack(dir, true);
            // });
            random.RandomList.Add(() =>
            {
                var dir = InGame.Player.Position - Position;
                attack.TripleAttack(dir, true);
            });
            random.SetTarget<WaitState>();

            var wait = _enemyAi.AddState<WaitState>();
            wait.SetTarget<ChaseState>();

            _enemyAi.InitState<IdleState>();
            AddAct(_enemyAi);
        }
    }
}