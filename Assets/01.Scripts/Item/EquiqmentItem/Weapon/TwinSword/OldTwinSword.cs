using Actors.Characters;
using Core;
using System.Collections;
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
		if (_isCoolTime)
			return;

		Vector3 vector = InGame.CamDirCheck(DirReturn(vec));
		_isCoolTime = true;
		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("TwinSword-Slash");
		obj.transform.position = _characterActor.Position + vector + Vector3.up;
		for (int i = 0; i < 6; i++)
		{
			//Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + vector, info.Atk, i * 0.2f, _characterActor, MovementType.None, true);
			InGame.Attack(_characterActor.Position + vector, new Vector3(1, 0, 1), info.Atk, i * 0.2f, _characterActor, true);
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
