using Actor.Acts;
using Managements.Managers;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
        protected override void Start()
        {
            base.Start();
            OnMove += GetAct<ActorMove>().Translate;
            OnChange += GetAct<PlayerActorChange>().Change;

			InputManager.OnChangePress += () => { OnChange?.Invoke(); };
			InputManager.OnMovePress += (pos) => { OnMove?.Invoke(pos, weapon); };
			InputManager.OnAttackPress += (pos) => { OnAttack?.Invoke(pos, weapon.AttackInfo); };
		}
    }
}