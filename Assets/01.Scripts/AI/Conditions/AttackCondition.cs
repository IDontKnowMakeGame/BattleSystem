using Actors.Bases;
using Actors.Characters;

namespace AI.Conditions
{
    public class AttackCondition : AiCondition
    {
        public Actor BaseActor;
        public override bool IsSatisfied()
        {
            var character = BaseActor as CharacterActor;
            if (character == null)
                return false;
            var result = !character.HasState(CharacterState.Attack);
            return result;
        }
    }
}