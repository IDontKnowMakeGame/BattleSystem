using Actors.Characters;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldStraightSword : StraightSword
{
	public override void Skill()
	{
		base.Skill();
		_characterActor.AddState(CharacterState.Skill);
		//InputManager<OldStraightSword>.OnMovePress += 
	}
}
