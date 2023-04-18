using Actors.Bases;

namespace AI.Conditions
{
    public class  BlockCondition : AiCondition
    {
        public int Area;
        public Actor TargetActor;
        public override bool IsSatisfied()
        {
            for (var i = -Area; i <= Area; i++)
            {
                for (var j = -Area; j <= Area; j++)
                {
                    if (_thisActor.Position.x + i == TargetActor.Position.x &&
                        _thisActor.Position.z + j == TargetActor.Position.z)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}