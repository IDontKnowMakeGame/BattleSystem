using System.Collections.Generic;
using Actor.Acts;
using Managements.Managers;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
        protected override void Start()
        {
            OnMove += GetAct<ActorMove>().Translate;
            OnAttack += GetAct<ActorAttack>().Attack;
            OnChange += GetAct<ActorChange>().Change;
            base.Start();
		}
    }
}