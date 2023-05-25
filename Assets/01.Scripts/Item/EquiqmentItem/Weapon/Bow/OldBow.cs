using Actors.Characters;
using Acts.Characters.Player;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBow : Bow
{
	public override void Skill(Vector3 vec)
	{
		if (!_characterActor.HasState(CharacterState.Hold))
			return;
		if (_isCoolTime)
			return;

		_isCoolTime = true;
		_characterActor.GetAct<PlayerMove>().IsSKill = true;
		_characterActor.GetAct<PlayerMove>().BowBackStep(_characterActor.Position + -_currentVec);
		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Dust");
		obj.transform.position = _characterActor.Position + Vector3.up;
		obj.transform.localRotation = Quaternion.LookRotation(_characterActor.Position + -_currentVec);
		//PlayerMove.OnMoveEnd += MoveEnd;
	}

	private void MoveEnd(int id, Vector3 vec)
	{
		if (id != _characterActor.UUID) return;

		//GameObject obj = Define.GetManager<ResourceManager>().Instantiate("Dust");
		//obj.transform.position = _characterActor.Position + Vector3.up;
		//obj.transform.localRotation = Quaternion.LookRotation(_characterActor.Position + _currentVec);
		PlayerMove.OnMoveEnd -= MoveEnd;
	}
}
