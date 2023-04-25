﻿using System.Reflection;
using Acts.Characters;
using Acts.Characters.Enemy;
using Acts.Characters.Enemy.Boss.CrazyGhost;
using AI;
using AI.Conditions;
using AI.States;                                                                                                                                                
using Blocks.Acts;
using Core;
using Tool.Map.Room;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors.Characters.Enemy.CrazyGhost
{
    public class CrazyGhostActor : BossActor
    {
        private PatternState patternState;
        [SerializeField] private EnemyParticle particle;
        [SerializeField] Room bossRoom;
        protected override void Init()
        {
            base.Init(); 
            AddAct(_enemyAi);
            AddAct(particle);
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
            var jump = _enemyAi.GetState<JumpState>();
            var triple = _enemyAi.GetState<TripleState>();
            var area = _enemyAi.GetState<AreaState>();
            var screaming = _enemyAi.GetState<ScreamingState>();
            var secondPhase = _enemyAi.GetState<SecondPhaseState>();
            chase.OnStay += () => { move.Chase(InGame.Player); };
            pattern.RandomActions.Add(() =>
            {
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                var dir = (Position - playerPos).GetDirection();
                Attack(dir, "Slash", () => { attack.HorizontalAttack(playerPos, false); });
            });
            pattern.RandomActions.Add(() =>
            {
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                var dir = (Position - playerPos).GetDirection();
                Attack(dir,"Pierce", () => { attack.VerticalAttack(playerPos, false); });
            });
            jump.OnEnter = () =>
            {
                AddState(CharacterState.Attack);
                canKnockBack = true;
                var jumpClip = _enemyAnimation.GetClip("JumpAttackJump");
                var readyClip = _enemyAnimation.GetClip("JumpAttackReady");
                _enemyAnimation.Play("JumpAttackReady");
                readyClip.OnExit = () =>
                {
                    var playerPos = InGame.Player.Position;
                    var dir = (Position - playerPos).GetDirection();
                    _enemyAnimation.Play("JumpAttackJump");
                    move.Jump(playerPos, dir, 0);
                    jumpClip.OnExit = () =>
                    {
                        canKnockBack = false;
                        AttackWithNoReady(Vector3.zero, "JumpAttack", () => { attack.RoundAttack(1, false); });
                    };
                };
            };
            triple.OnEnter = () =>
            {
                var playerPos = InGame.Player.Position;
                var dir = (Position - playerPos).GetDirection();
                AddState(CharacterState.Attack);
                AttackWithNoReturn(dir,"Combo1", () =>
                {
                    attack.HorizontalAttack(playerPos, false);
                }, () =>
                {
                    AttackWithNoReturn(dir,"Combo2", () =>
                    {
                        attack.VerticalAttack(playerPos, false);
                    }, () =>
                    {
                        Attack(dir,"Combo3", () =>
                        {
                            attack.ForwardAttack(playerPos, false);
                        });
                    });
                });
            };
            soulAttack.OnEnter = () =>
            {
                AddState(CharacterState.Attack);
                var jumpClip = _enemyAnimation.GetClip("SoulAttackJump");
                jumpClip.OnExit = null;
                _enemyAnimation.Play("SoulAttackJump");
                var dir = (Position - InGame.Player.Position).GetDirection();
                var playerPos = InGame.Player.Position;
                move.Jump(Position, dir, 3);
                jumpClip.OnExit += () =>
                {
                    Attack(Vector3.zero,"SoulAttack", () => { attack.SoulAttack(playerPos, 0.15f); }, false);
                };
            };
            screaming.OnEnter = () =>
            {
                AddState(CharacterState.Attack);
                var jumpClip = _enemyAnimation.GetClip("SoulAttackJump");
                jumpClip.OnExit = null;
                _enemyAnimation.Play("SoulAttackJump");
                var centerPos = (bossRoom.EndPos + bossRoom.StartPos) * 0.5f;
                move.Jump(centerPos, Vector3.zero, 0);
                jumpClip.OnExit += () =>
                {
                    Attack(Vector3.zero,"SoulAttack", () => { attack.AreaAttack(10, true); }, false);
                };
            };
            secondPhase.OnEnter = () =>
            {
                AddState(CharacterState.Attack);
                var jumpClip = _enemyAnimation.GetClip("SoulAttackJump");
                _enemyAnimation.Play("SoulAttackJump");
                var dir = (Position - InGame.Player.Position).GetDirection();
                var playerPos = InGame.Player.Position;
                move.Jump(Position, dir, 3);
                jumpClip.OnExit += () =>
                {
                    GetAct<EnemyParticle>().PlaySecondPhaseParticle();
                    Attack(Vector3.zero,"SoulAttack", () => { attack.SoulAttack(playerPos, 0f); }, false);
                };
            };

            area.OnEnter = () =>
            {
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                Attack(Vector3.zero, "AreaAttack", () => { attack.AreaAttack(5, false); }, false);
            };
        }
    }
}