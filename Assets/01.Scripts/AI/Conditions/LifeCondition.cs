using Actors.Bases;
using Actors.Characters;

namespace AI.Conditions
{
    public class LifeCondition : AiCondition
    {
        public Actor BaseActor;
        public float TargetLife;
        public override bool IsSatisfied()
        {
            var character = BaseActor as CharacterActor;
            if (character == null)
                return false;
            var stat = character.GetAct<CharacterStatAct>();
            var maxHp = stat.BaseStat.hp;
            var hp = stat.ChangeStat.hp;
            var result = (hp / maxHp) * 100 > TargetLife;
            return result;
        }
    }
}