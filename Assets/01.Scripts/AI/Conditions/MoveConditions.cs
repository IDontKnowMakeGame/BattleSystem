using Actors.Characters;

namespace AI.Conditions
{
    public class MoveCondition : AiCondition
    {
        private CharacterActor CharacterActor => _thisActor as CharacterActor;
        public override bool IsSatisfied()
        {
            return CharacterActor.HasState(CharacterState.Move);
        }
    }
}