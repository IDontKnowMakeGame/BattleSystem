using Acts.Characters.Enemy;

namespace AI.Conditions
{
    public class EnteredCondition : AiCondition
    {
        public string StateName;
        private AiState currentState;

        public override bool IsSatisfied()
        {
            if (currentState == null)
            {
                
                currentState = _thisActor.GetAct<EnemyAI>().GetState(StateName);
            }

            return currentState.HasPlayed;
        }
    }
}