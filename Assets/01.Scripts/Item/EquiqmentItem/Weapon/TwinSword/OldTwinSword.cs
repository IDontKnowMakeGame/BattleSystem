using Actors.Characters;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTwinSword : TwinSword
{
	public override void Skill()
	{
		base.Skill();
		if (_characterActor.HasState(CharacterState.Skill))
			return;

		_characterActor.AddState(CharacterState.Skill);
		InputManager<TwinSword>.OnMovePress += Skill;
	}

	private void Skill(Vector3 vec)
	{
		InputManager<TwinSword>.OnMovePress -= Skill;
		_isCoolTime = true;
		for(int i = 0; i<6; i++)
		{
			Define.GetManager<MapManager>().AttackBlock(vec, info.Atk, 1f, _playerActor);
		}
		_characterActor.StartCoroutine(SkillCorutine(vec));
	}

	private IEnumerator SkillCorutine(Vector3 vec)
	{
		yield return new WaitForSeconds(1f);
		_characterActor.RemoveState(CharacterState.Skill);
	}
}
