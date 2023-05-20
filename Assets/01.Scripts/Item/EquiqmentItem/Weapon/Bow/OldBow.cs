using Actors.Characters;
using Acts.Characters.Player;
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
	}
}
