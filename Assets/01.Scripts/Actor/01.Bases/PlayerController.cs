using System.Collections.Generic;
using Actor.Acts;
using Managements.Managers;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
        protected void Start()
        {
            OnMove += GetAct<ActorMove>().Translate;
            OnChange += GetAct<ActorChange>().Change;
        }
    }
}