using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
public class OldSpear : BaseSpear
{
	float beforeAtk;
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldSpear");
	}

	public override void ChangeKey()
	{
		base.ChangeKey();
		_playerAttack.onBehaviourEnd = CountUp;
	}
	protected override void Skill(Vector3 vec)
	{
		count++;

		Debug.Log("때리기");
		if (count == 3)
		{
			Debug.Log("터지기");
			beforeAtk = _weaponStats.Atk;
			_weaponStats.Atk *= 2;
		}
		else if (count == 4)
		{
			count = 1;
			_weaponStats.Atk = beforeAtk;
		}
	}

	private void CountUp() => Skill(Vector3.zero);
}
