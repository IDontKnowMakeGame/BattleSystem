using Units.Base.Player.AI.States.Enemy.Common.ElderGhostOfBow;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Units.Base.Enemy.Common
{
    public class BowElderGhostBase : CommonEnemyBase
    {
        [SerializeField] private UnitEquiq _enemyWeapons;
        [SerializeField] private CharacterRender characterRender;
        protected override void Init()
        {
            AddBehaviour(ThisStat);
            AddBehaviour(_enemyWeapons);
            AddBehaviour<EnemyMove>();
            var fsm = AddBehaviour<UnitFSM>();
            fsm.SetDefaultState<IdleState>();
            base.Init();
            ChangeBehaviour(characterRender);
        }
    }
}