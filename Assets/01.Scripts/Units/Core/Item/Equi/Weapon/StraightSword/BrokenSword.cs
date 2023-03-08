using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSword : BaseStraightSword
{
	private int count = 0;
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.OnSkillPress -= Skill;
		attackEndAction = CountUp;
	}

	private void CountUp()
	{
		count++;
		if(count == 99)
		{
			Skill();
		}
		else if(count == 100)
		{
			count = 0;
			GetWeaponStateData("brokenSword");
		}
	}
	protected override void Skill()
	{
		_weaponStats.Atk = 9999999;
	}

	public override void Reset()
	{
		base.Reset();
		count = 0;
		attackEndAction = null;
	}
}
