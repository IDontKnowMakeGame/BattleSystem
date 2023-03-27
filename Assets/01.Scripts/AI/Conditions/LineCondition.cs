using Actors.Bases;

namespace AI.Conditions
{
    public class LineCondition : AiCondition
    {
        public Actor TargetActor;

        public override bool IsSatisfied()
        {
            return _thisActor.Position.IsLine(TargetActor.Position);
        }
    }
}