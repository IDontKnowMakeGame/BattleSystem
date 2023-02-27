using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenSword : BaseStraightSword
{
	private int count = 0;

	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("brokenSword");
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.OnSkillPress -= Skill;
		_playerAttack.onBehaviourEnd = CountUp;
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
		_weaponStats.Atk = 900000;
	}

	public override void Reset()
	{
		base.Reset();
		count = 0;
		_playerAttack.onBehaviourEnd = null;
	}
}
