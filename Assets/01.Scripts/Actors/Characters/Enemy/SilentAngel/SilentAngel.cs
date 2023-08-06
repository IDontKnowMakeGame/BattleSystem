using Acts.Characters;
using Acts.Characters.Enemy.Boss.SilentAngel;
using AI.States;
using UnityEngine;

namespace Actors.Characters.Enemy.SilentAngel
{
    public class SilentAngel : BossActor
    {
        private CharacterMove move = null;
        private SilentAngelAttack attack;
        
        protected override void Init()
        {
            base.Init();
            AddAct(_enemyAi);
            move = AddAct<CharacterMove>();
            attack = AddAct<SilentAngelAttack>();
        }

        protected override void Start()
        {
            base.Start();
            var slash = _enemyAi.GetState<SlashState>();

            slash.OnEnter += () =>
            {
                Attack(Vector3.zero, "CircleSlash", () =>
                {
                    attack.RoundAttack(2, false);
                });
            };
        }
    }
}