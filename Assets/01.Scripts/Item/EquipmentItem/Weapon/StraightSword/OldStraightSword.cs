using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldStraightSword : BaseStriaghtSword
{
	public override void Skill()
	{
		_acotrController.OnMove +=  Move;
	}

	public void Move(Vector3 vec, Weapon weapon)
	{

	}
}
