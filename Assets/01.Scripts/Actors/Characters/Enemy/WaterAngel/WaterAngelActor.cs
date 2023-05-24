using Acts.Characters;
using Acts.Characters.Enemy;
using Acts.Characters.Enemy.Boss.WaterAngel;

namespace Actors.Characters.Enemy.WaterAngel
{
    public class WaterAngelActor : BossActor
    {
        private CharacterMove move = null;
        private WaterAngelAttack attack;
        protected override void Init()
        {
            base.Init();
            AddAct(_enemyAi);
            move = AddAct<CharacterMove>();
            attack = AddAct<WaterAngelAttack>();
        }

        protected override void Start()
        {
            base.Start();
            
            
        }
    }
}