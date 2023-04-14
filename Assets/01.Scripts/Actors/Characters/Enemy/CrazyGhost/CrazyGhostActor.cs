using System.Reflection;
using Acts.Characters;
using Acts.Characters.Enemy;
using Acts.Characters.Enemy.Boss.CrazyGhost;
using AI;
using AI.Conditions;
using AI.States;                                                                                                                                                
using Blocks.Acts;
using Core;
using Unity.VisualScripting;
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
            var soulAttack = _enemyAi.GetState<SoulAttackState>();
            chase.OnStay += () => { move.Chase(InGame.Player); };
            pattern.RandomActions.Add(() =>
            {
                Attack("LowerSlash", () => { attack.HorizontalAttack(InGame.Player.Position); });
            });
            pattern.RandomActions.Add(() =>
            {
                Attack("LowerPierce", () => { attack.VerticalAttack(InGame.Player.Position); });
            });
            soulAttack.OnEnter = () =>
            {
                AddState(CharacterState.Attack);
                var jumpClip = _enemyAnimation.GetClip("SoulAttackJump");
                _enemyAnimation.Play("SoulAttackJump");
                var dir = (Position - InGame.Player.Position).GetDirection();
                var playerPos = InGame.Player.Position;
                move.Jump(dir, 3);
                jumpClip.OnExit += () =>
                {
                    Attack("SoulAttack", () => { attack.SoulAttack(playerPos); });
                };
            };
        }
    }
}