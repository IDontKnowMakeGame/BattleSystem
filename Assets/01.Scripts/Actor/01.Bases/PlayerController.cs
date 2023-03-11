using System.Collections.Generic;
using Actor.Acts;
using Managements.Managers;
using UnityEngine;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
        
        protected override void Start()
        {
            OnMove += GetAct<ActorMove>().Translate;
            OnChange += GetAct<ActorChange>().Change;
            base.Start();
		}
    }
}