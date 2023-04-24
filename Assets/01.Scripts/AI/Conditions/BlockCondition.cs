using Actors.Bases;

namespace AI.Conditions
{
    public class  BlockCondition : TargetCondition
    {
        public int Area;
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