using Acts.Characters;
using Acts.Characters.Enemy;
using Acts.Characters.Enemy.Boss.KnightStatue;
using AI.States;
using Core;
using UnityEngine;

namespace Actors.Characters.Enemy.KnightStatue
{
    public class KnightStatueActor : BossActor
    {
        [SerializeField] private EnemyParticle particle;

        protected override void Init()
        {
            base.Init(); 
            AddAct(_enemyAi);
            AddAct(particle);
            AddAct<CharacterMove>();
            AddAct<KnightStatueAttack>();
        }

        protected override void Start()
        {
            base.Start();
            _enemyAi.CurrentState = _enemyAi.GetState<WaitState>();
            var move = GetAct<CharacterMove>();
            var attack = GetAct<KnightStatueAttack>();
            var pattern = _enemyAi.GetState<PatternState>();
            var jump = _enemyAi.GetState<JumpState>();
            
            pattern.RandomActions.Add(new NextAction(() =>
            {
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                var dir = (playerPos - Position).GetDirection();
                if (dir.magnitude > 1)
                {
                    RemoveState(CharacterState.Attack);
                    return;
                }
                Attack(dir, "Slash", () =>
                {
                    attack.SizeAttack(dir, new Vector3(3, 0, 2), 1.5f, false);
                });
            }, 40f));
            pattern.RandomActions.Add(new NextAction(() =>
            {
                AddState(CharacterState.Attack);
                var playerPos = InGame.Player.Position;
                var dir = (playerPos - Position).GetDirection();
                if (dir.magnitude > 1)
                {
                    RemoveState(CharacterState.Attack);
                    return;
                }
                Attack(dir,"Strike", () =>
                {
                    move.Jump(Position, dir, 5, 0f);
                    attack.SizeAttack(dir, new Vector3(1, 0, 4), 2.5f, false);

                });
            }, 60f));
            
            jump.OnEnter = () =>
            {
                attack.JumpAttack(1, false);
            };
        }
    }
}