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

        private PatternState patternState;
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
            var chase = _enemyAi.GetState<ChaseState>();
            chase.OnStay += () =>
            {
                move.Chase(InGame.Player);
            };
            
            SetPattern();

            var secondPhase = _enemyAi.GetState<SecondPhaseState>();
            secondPhase.OnEnter += () =>
            {
                patternState.RandomActions.Clear();
                SetPattern();
                patternState.RandomActions.Add(() =>
                {
                    var attack = GetAct<EnemyAttack>();
                    var dir = InGame.Player.Position - Position;
                    attack.SoulAttack(dir, true);
                });
            };
        }

        private void SetPattern()
        {
            patternState = _enemyAi.GetState<PatternState>();
            var attack = GetAct<EnemyAttack>();
            patternState.RandomActions.Add(() =>
            {
                var dir = InGame.Player.Position - Position;
                attack.ForwardAttack(dir, true);
            });
            patternState.RandomActions.Add(() =>
            {
                var dir = InGame.Player.Position - Position;
                attack.BackAttack(dir, true);
            });
            patternState.RandomActions.Add(() =>
            {
                var dir = InGame.Player.Position - Position;
                attack.TripleAttack(dir, true);
            });
        }
    }
}