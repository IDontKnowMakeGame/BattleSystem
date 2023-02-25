using Core;
using Unit.Base.AI;
using Unit.Enemy.AI.Conditions;
using Units.Base.Unit;

namespace _01.Scripts.Units.AI.States.Enemy.Common.ElderCommonGhost
{
    public class IdleState : AIState
    {
        public override void Awake()
        {
            var toChase = new AITransition();
            var squareCheck = new SquareCheckCondition();
            squareCheck.SetUnits(InGame.PlayerBase, ThisBase as UnitBase);
            squareCheck.SetResult(true);
            squareCheck.SetDistance(1);
            toChase.AddCondition(squareCheck);
            toChase.SetTarget(new ChaseState());
            AddTransition(toChase); 
        }
    }
}