using Actor.Acts;
using Managements.Managers;
using System;

namespace Actor.Bases
{
    public class PlayerController : ActorController
    {
		public Action OnSkill = null;
		protected override void Start()
		{
			base.Start();
			OnMove += GetAct<ActorMove>().Translate;
			OnChange += GetAct<PlayerActorChange>().Change;
			OnSkill = weapon.Skill;

			InputManager.OnChangePress += () => { OnChange?.Invoke(); };
			InputManager.OnMovePress += (pos) => { OnMove?.Invoke(pos, weapon); };
			InputManager.OnAttackPress += (pos) => { OnAttack?.Invoke(pos, weapon); };
			InputManager.OnSkillPress += OnSkill;
		}
     }
}