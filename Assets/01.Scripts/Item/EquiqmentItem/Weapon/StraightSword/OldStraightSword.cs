using Actors.Characters;
using Acts.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldStraightSword : StraightSword
{
	public override void Skill()
	{
		if (_isCoolTime)
			return;
		if (_characterActor.HasState(~CharacterState.None & ~CharacterState.Move))
			return;

		_characterActor.AddState(CharacterState.Skill);
		_characterActor.StartCoroutine(SameTimeInput());
	}

	protected override void STimeInputSkill(Vector3 vec)
	{
		CharacterMove.OnMoveEnd += SkillInputEnd;
		_characterActor.RemoveState(CharacterState.Skill);
		PlayerMove move = _characterActor.GetAct<PlayerMove>();
		move.IsSKill = true;
		move.SkillDir = vec;
		move.Move(_characterActor.Position + InGame.CamDirCheck(vec) * 2);
	}

	protected override void SkillInputEnd(int i, Vector3 vec)
	{
		if (i != _characterActor.UUID)
			return;


		base.SkillInputEnd(i, vec);
		_characterActor.GetAct<PlayerMove>().IsSKill = false;
		CharacterMove.OnMoveEnd -= SkillInputEnd;
	}
}
