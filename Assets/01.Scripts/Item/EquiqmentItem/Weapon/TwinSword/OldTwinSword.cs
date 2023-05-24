using Actors.Characters;
using Core;
using System.Collections;
using Acts.Characters.Player;
using UnityEngine;

public class OldTwinSword : TwinSword
{
	public override void Skill(Vector3 vec)
	{
		if (_characterActor.HasState(CharacterState.Skill))
			return;
		if (_isCoolTime)
			return;

		_characterActor.AddState(CharacterState.Skill);
		Vector3 vector = InGame.CamDirCheck(vec);
		_isCoolTime = true;
		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("TwinSword-Slash");
		obj.transform.position = _characterActor.Position + vector + Vector3.up;
		obj.transform.localRotation = Quaternion.LookRotation(vector);
		_characterActor.GetAct<PlayerAnimation>().Play("Skill");

		_vec = _characterActor.transform.position;
		for (int i = 0; i < 6; i++)
		{
			//Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + vector, info.Atk, i * 0.2f, _characterActor, MovementType.None, true);
			_characterActor.StartCoroutine(SkillsCorutine(vector, 0.2f * i));
		}
		_characterActor.StartCoroutine(SkillCorutine());
	}

	Vector3 _vec = Vector3.zero;

	private IEnumerator SkillsCorutine(Vector3 vector, float speed)
	{
		yield return new WaitForSeconds(speed);
		InGame.Attack(_vec + vector, new Vector3(1, 0, 1), info.Atk, 0f, _characterActor, true);
	}

	private IEnumerator SkillCorutine()
	{
		yield return new WaitForSeconds(0.17f);
		_characterActor.RemoveState(CharacterState.Skill);
	}
}
