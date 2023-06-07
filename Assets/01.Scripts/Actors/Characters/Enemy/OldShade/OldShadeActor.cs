using System.Collections.Generic;
using System.Reflection;
using Acts.Characters;
using Acts.Characters.Enemy;
using AI.States;
using AttackDecals;
using Core;
using UnityEngine;

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

        // ReSharper disable Unity.PerformanceAnalysis
        // ReSharper disable Unity.PerformanceAnalysis
        protected override void Start()
        {
            base.Start();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<EnemyAttack>();
            var chase = _enemyAi.GetState<ChaseState>();
            var pattern = _enemyAi.GetState<PatternState>();
            
            chase.OnStay += () =>
            {
                move?.Chase(InGame.Player);
            };
            pattern.RandomActions.Add(() =>
            {
                if (HasState(CharacterState.Attack))
                    return;
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                var dir = (playerPos - Position).GetDirection();
                
                
                var dirName = GetDirName(dir);
                var nextState = dirName;
                var readyClip =  _enemyAnimation.GetClip( nextState + "Ready");
                var attackClip = _enemyAnimation.GetClip( nextState + "Attack");
                var returnClip = _enemyAnimation.GetClip( nextState + "Return");
                if (readyClip == null || attackClip == null || returnClip == null)
                {
                    nextState = "Lower";
                    readyClip =  _enemyAnimation.GetClip( nextState + "Ready");
                    attackClip = _enemyAnimation.GetClip( nextState + "Attack");
                    returnClip = _enemyAnimation.GetClip( nextState + "Return");
                }

                
                var delay = readyClip.delay * (readyClip.fps + attackClip.fps);
                readyClip.OnEnter = () =>
                {
                    attack.HorizontalAttackWithDelay(dir, delay, false);
                };
                readyClip.OnExit = () =>
                {
                    _enemyAnimation.Play( nextState + "Attack");
                };
                attackClip.OnExit = () =>
                {
                    _enemyAnimation.Play( nextState + "Return");    
                };
                returnClip.OnExit = () =>
                {
                    RemoveState(CharacterState.Attack);
                };
                _enemyAnimation.Play( nextState + "Ready");
            });
        }
    }
}