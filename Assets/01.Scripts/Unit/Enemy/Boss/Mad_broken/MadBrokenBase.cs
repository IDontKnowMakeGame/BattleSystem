using Unit.Enemy.AI.MadBroken.State;
using Unit.Enemy.Base;

namespace Unit.Boss.MadBroken
{
    public class MadBrokenBase : EnemyBase
    {
        protected override void Init()
        {
            base.Init();
            AddBehaviour<EnemyFSM>().SetDefaultState<ChaseState>();
            AddBehaviour<EnemyMove>();
            AddBehaviour<EnemyAttack>();
        }
    }
}