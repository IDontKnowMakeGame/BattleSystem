using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
public class OldSpear : BaseSpear
{
	public override void Start()
	{
		base.Start();
		GetWeaponStateData("oldSpear");
	}

	public override void Update()
	{
		base.Update();
		Skill(Vector3.zero);
	}
	protected override void Skill(Vector3 vec)
	{
		if(count == 3)
		{
			count = 0;
			_unitAttack.Attack(_currentAttackPos);
		}
	}
}
