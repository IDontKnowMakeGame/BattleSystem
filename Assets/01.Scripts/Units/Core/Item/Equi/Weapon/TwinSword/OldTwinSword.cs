using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
public class OldTwinSword : BaseTwinSword
{
	public override void Start()
	{
		base.Start();
		GetWeaponStateData("oldTwinSword");
		_inputManager.ChangeInGameAction(InputTarget.Skill, InputStatus.Press, () => Skill(Vector3.zero));
	}
	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		isSkill = true;
		SixTimeAttak(vec);
	}

	private void SixTimeAttak(Vector3 dir)
	{
		_isCoolTime = true;
		for (int i = 0; i < 6; i++)
		{
			_unitAttack.Attack(dir);
			_unitAttack.onBehaviourEnd = waitReset;
		}
	}

	private void waitReset()
	{
		isSkill = false;
	}
}
