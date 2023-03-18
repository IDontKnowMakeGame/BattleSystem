using Acts.Characters;
using Acts.Characters.Enemy;
using AI;
using AI.Conditions;
using AI.States;
using UnityEngine;

namespace Actors.Characters.Enemy.CrazyGhost
{
    public class CrazyGhostActor : EnemyActor
    {
        protected override void Init()
        {
            base.Init(); 
            var idle = _enemyAi.AddState<IdleState>();
            var chase = _enemyAi.AddState<ChaseState>();
            var move = AddAct<CharacterMove>();

            chase.SetTarget<IdleState>();
            idle.SetTarget<ChaseState>();
            
            _enemyAi.InitState<IdleState>();
            AddAct(_enemyAi);
        }
    }
}