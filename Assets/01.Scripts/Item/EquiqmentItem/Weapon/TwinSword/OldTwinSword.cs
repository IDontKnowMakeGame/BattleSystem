using Actors.Characters;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Blocks.Acts;
using Acts.Characters.Player;
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
		_characterActor.StartCoroutine(SameTimeInput());
	}

	protected override void STimeInputSkill(Vector3 vec)
	{
		Vector3 vector = InGame.CamDirCheck(vec);
		for (int i = 0; i < 6; i++)
		{
			Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + vector, info.Atk, i * 0.2f, _characterActor, MovementType.None, true);
			_characterActor.GetAct<PlayerAnimation>().Play("Skill");
		}
		_characterActor.StartCoroutine(SkillCorutine());
	}

	private IEnumerator SkillCorutine()
	{
		yield return new WaitForSeconds(0.17f);
		_characterActor.RemoveState(CharacterState.Skill);
	}
}
