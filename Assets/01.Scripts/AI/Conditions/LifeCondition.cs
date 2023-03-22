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
            var stat = character.GetAct<CharacterStatAct>();
            var maxHp = stat.BaseStat.hp;
            var hp = stat.ChangeStat.hp;
            var result = (hp / maxHp) * 100 > floatParam;
            return result;
        }
    }
}