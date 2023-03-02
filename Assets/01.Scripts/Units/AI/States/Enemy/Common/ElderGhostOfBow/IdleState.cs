using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.AI.States.Enemy.Attack;
using Units.Base.Unit;

namespace Units.Base.Player.AI.States.Enemy.Common.ElderGhostOfBow
{
    public class IdleState : AIState
    {
        public override void Awake()
        {
            var toAttack = new AITransition();
            var lineCheck = new LineDetectCondition();
            lineCheck.SetResult(true);
            lineCheck.SetUnits(InGame.PlayerBase, ThisBase as UnitBase);
            lineCheck.SetDistance(5);
            toAttack.AddCondition(lineCheck);
            var attack = new BowAttack
            {
                NextState = new IdleState()
            };
            toAttack.SetTarget(attack);
            AddTransition(toAttack);
        }
    }
}