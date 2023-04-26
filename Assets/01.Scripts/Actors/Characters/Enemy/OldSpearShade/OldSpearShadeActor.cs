using System.Linq;
using Acts.Characters;
using Acts.Characters.Enemy;
using AI;
using AI.Conditions;
using AI.States;
using Core;
using UnityEngine;

namespace Actors.Characters.Enemy.OldSpearShade
{
    public class OldSpearShadeActor : EnemyActor
    {
        [SerializeField] private AiTransition toIdle;
        private MovableCondition movableCondition;
        protected override void Init()
        {
            base.Init();
            AddAct<EnemyAttack>();
            AddAct<CharacterMove>();
            AddAct(_enemyAi);

            movableCondition = toIdle._conditions.Where((c) => c is MovableCondition).FirstOrDefault() as MovableCondition;
        }

        protected override void Start()
        {
            base.Start();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<EnemyAttack>();
            var idle = _enemyAi.GetState<IdleState>();
            var chase = _enemyAi.GetState<ChaseState>();
            Vector3 dir = Vector3.zero;
            idle.OnEnter = () =>
            {
                _enemyAnimation.Play("Idle");
            };
            chase.OnEnter = ()=>
            {
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                dir = (playerPos - Position).GetDirection();
                dir = InGame.CamDirCheck(dir);
                movableCondition.nextDir = dir;
                var dirName = GetDirName(dir);
                var readyClip = _enemyAnimation.GetClip(dirName + "Ready");
                var moveClip = _enemyAnimation.GetClip(dirName + "Move");
                _enemyAnimation.Play(dirName + "Ready");
                readyClip.SetEventOnFrame(0, () =>
                {
                    attack.DefaultAttack(dir, false);
                });
                readyClip.OnExit = () =>
                {
                    AddState(CharacterState.Attack);
                    _enemyAnimation.Play(dirName + "Move");
                    move.Translate(dir);
                    moveClip.OnExit = () =>
                    {
                        RemoveState(CharacterState.Attack);
                        _enemyAnimation.Play(dirName + "Ready");
                    };
                };
            };
            
        }
    }
}