using Actor.Bases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldBow : Bow
{
	private Vector3 _currentVec = Vector3.zero;

	public override void Init(ActorController actContorller)
	{
		base.Init(actContorller);
		_actController.OnAttack += Vec;
	}
	public override void Skill()
	{
		_actController.OnMove(-_currentVec, _actController.weapon);
	}

	private void Vec(Vector3 vec, Weapon info)
	{
		_currentVec = vec;
	}
}
