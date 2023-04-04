using Actors.Characters;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Blocks.Acts;
using UnityEngine;

public class OldTwinSword : TwinSword
{
	public override void Skill()
	{
		if (_characterActor.HasState(CharacterState.Skill))
			return;
		if (_isCoolTime)
			return;
		_characterActor.AddState(CharacterState.Skill);
		InputManager<TwinSword>.OnMovePress += Skill;
	}

	private void Skill(Vector3 vec)
	{
		InputManager<TwinSword>.OnMovePress -= Skill;
		_isCoolTime = true;
		Vector3 vector = InGame.CamDirCheck(vec);
		for(int i = 0; i<6; i++)
		{
			Define.GetManager<MapManager>().AttackBlock(_characterActor.Position+vector, info.Atk, 1f, _characterActor, MovementType.None, true);
		}
		_characterActor.StartCoroutine(SkillCorutine());
	}

	private IEnumerator SkillCorutine()
	{
		yield return new WaitForSeconds(1f);
		_characterActor.RemoveState(CharacterState.Skill);
	}
}
