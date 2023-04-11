using System.Reflection;
using Acts.Characters;
using Acts.Characters.Enemy;
using Acts.Characters.Enemy.Boss.CrazyGhost;
using AI;
using AI.Conditions;
using AI.States;
using Blocks.Acts;
using Core;
using UnityEngine;

namespace Actors.Characters.Enemy.CrazyGhost
{
    public class CrazyGhostActor : BossActor
    {

        private PatternState patternState;
        protected override void Init()
        {
            base.Init(); 
            AddAct(_enemyAi);
            AddAct<CharacterMove>();
            AddAct<CrazyGhostAttack>();
        }

        protected override void Start()
        {
            base.Start();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<CrazyGhostAttack>();
            var chase = _enemyAi.GetState<ChaseState>();
            var pattern = _enemyAi.GetState<PatternState>();
            chase.OnStay += () => { move.Chase(InGame.Player); };
            pattern.RandomActions.Add(() =>
            {
                attack.SoulAttack(InGame.Player.Position);
            });
        }
    }
}