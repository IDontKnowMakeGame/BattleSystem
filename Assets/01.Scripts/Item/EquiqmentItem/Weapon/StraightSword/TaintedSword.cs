using Actors.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaintedSword : StraightSword
{
	public override void Skill()
	{
		base.Skill();
		_characterActor.AddState(CharacterState.Skill);
		_isCoolTime = true;
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.forward, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.back, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.left, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.right, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.right + Vector3.forward, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.left + Vector3.forward, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.left + Vector3.back, info.Atk, 1f, _characterActor, true);
		Define.GetManager<MapManager>().AttackBlock(_characterActor.Position + Vector3.right + Vector3.back, info.Atk, 1f, _characterActor, true);
		_characterActor.StartCoroutine(SkillCorutine());
	}
	private IEnumerator SkillCorutine()
	{
		yield return new WaitForSeconds(1f);
		_characterActor.RemoveState(CharacterState.Skill);
	}
}
