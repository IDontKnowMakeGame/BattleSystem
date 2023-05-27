using Actors.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Blocks.Acts;
using UnityEngine;

public class TaintedSword : StraightSword
{
	public override void Skill(Vector3 vec)
	{
		if (_playerActor.HasAnyState()) return;

		base.Skill(vec);

		if (_isCoolTime)
			return;

		_characterActor.AddState(CharacterState.Skill);
		_isCoolTime = true;

		PlayerMove move = _characterActor.GetAct<PlayerMove>();
		move.SkillDir = vec;
		move.SkillAnimation();

		InGame.Attack(_characterActor.Position + Vector3.forward, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.back, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.left, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.right, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.right + Vector3.forward, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.left + Vector3.forward, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.left + Vector3.back, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		InGame.Attack(_characterActor.Position + Vector3.right + Vector3.back, new Vector3(1, 0, 1), info.Atk, 1f, _characterActor, true);
		
		_characterActor.StartCoroutine(SkillCorutine());
	}
	private IEnumerator SkillCorutine()
	{
		yield return new WaitForSeconds(1f);
		Define.GetManager<ResourceManager>().Instantiate("TaintedEffect").transform.position = _characterActor.Position + Vector3.up;
		_characterActor.RemoveState(CharacterState.Skill);
	}
}
