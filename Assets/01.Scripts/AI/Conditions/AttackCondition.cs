using Actors.Characters;

namespace AI.Conditions
{
    public class AttackCondition : AiCondition
    {
        public override bool IsSatisfied()
        {
            var character = actorParam as CharacterActor;
            if (character == null)
                return false;
            var result = !character.HasState(CharacterState.Attack);
            return result;
        }
    }
}