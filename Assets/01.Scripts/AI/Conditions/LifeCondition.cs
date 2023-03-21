using Actors.Characters;

namespace AI.Conditions
{
    public class LifeCondition : AiCondition
    {
        public override bool IsSatisfied()
        {
            var character = actorParam as CharacterActor;
            if (character == null)
                return false;
            var result = character.GetAct<CharacterStatAct>().ChangeStat.hp > floatParam;
            return result;
        }
    }
}