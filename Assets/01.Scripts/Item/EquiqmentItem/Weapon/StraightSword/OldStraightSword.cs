using Actors.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldStraightSword : StraightSword
{
	public override void Skill()
	{
		if (_isCoolTime)
			return;
		_characterActor.AddState(CharacterState.Skill);
		_characterActor.GetAct<PlayerMove>().IsSKill = true;
		_characterActor.GetAct<PlayerMove>().distance = 2;
		InputManager<StraightSword>.OnMovePress += SkillStart;
		PlayerMove.OnMoveEnd += SkillEnd;
	}

	private void SkillStart(Vector3 vec)
	{
		InputManager<OldStraightSword>.OnMovePress -= SkillStart;
		_characterActor.RemoveState(CharacterState.Skill);
	}
	private void SkillEnd(int id, Vector3 vec)
	{
		if (id != _characterActor.UUID)
			return;

		_characterActor.GetAct<PlayerMove>().distance = 1;
		_isCoolTime = true;
		_characterActor.GetAct<PlayerMove>().IsSKill = false;
		PlayerMove.OnMoveEnd -= SkillEnd;
	}
}
