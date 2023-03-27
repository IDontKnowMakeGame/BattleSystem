using Actors.Characters;
using Acts.Characters.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBow : Bow
{
	public override void Skill()
	{
		if (!_characterActor.HasState(CharacterState.Hold))
			return;

		_isCoolTime = true;
		_characterActor.GetAct<PlayerMove>().Move(_characterActor.Position + -_currentVec);
	}
}
