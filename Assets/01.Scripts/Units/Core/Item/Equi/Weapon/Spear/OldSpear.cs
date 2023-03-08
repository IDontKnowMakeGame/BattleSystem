using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using UnityEngine.Rendering.Universal;

public class OldSpear : BaseSpear
{
	float beforeAtk;
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.OnSkillPress -= Skill;
		attackEndAction = CountUp;
	}
	protected override void Skill()
	{
		count++;
		
		if (count == 3)
		{
			beforeAtk = _weaponStats.Atk;
			_weaponStats.Atk *= 2;
		}
		else if (count == 4)
		{
			count = 1;
			_weaponStats.Atk = beforeAtk;
		}
	}

	private void CountUp() => Skill();

	public override void Reset()
	{
		base.Reset();
		attackEndAction = null;
	}
}
