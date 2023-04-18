using System.Reflection;
using Acts.Characters;
using Acts.Characters.Enemy;
using AI.States;
using Core;

namespace Actors.Characters.Enemy.OldShade
{
    public class OldShadeActor : EnemyActor
    {
        protected override void Init()
        {
            base.Init();
            AddAct<EnemyAttack>();
            AddAct<CharacterMove>();
            AddAct(_enemyAi);
        }

        protected override void Start()
        {
            base.Start();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<EnemyAttack>();
            var chase = _enemyAi.GetState<ChaseState>();
            var pattern = _enemyAi.GetState<PatternState>();
            
            chase.OnStay += () =>
            {
                move.Chase(InGame.Player);
            };
            pattern.RandomActions.Add(() =>
            {
                var playerPos = InGame.Player.Position;
                var dir = (Position - playerPos).GetDirection();
                attack.HorizontalAttack(playerPos, true);
            });
            
        }
    }
}