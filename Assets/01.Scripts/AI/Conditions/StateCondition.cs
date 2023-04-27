using Actors.Characters;

namespace AI.Conditions
{
    public class StateCondition : AiCondition
    {
        public CharacterState State;
        
        public override bool IsSatisfied()
        {
            var character = _thisActor as CharacterActor;
            if (character == null)
                return false;
            var result = character.HasState(State);
            return result;
        }
    }
}