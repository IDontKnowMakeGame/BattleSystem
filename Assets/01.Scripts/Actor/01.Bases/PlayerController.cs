using System.Collections.Generic;
using Actor.Acts;
using Managements.Managers;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
        protected override void Awake()
        {
            OnMove += GetAct<ActorMove>().Translate;
            base.Awake();
        }
    }
}