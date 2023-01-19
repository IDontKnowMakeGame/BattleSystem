using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
public class OldTwinSword : Weapon
{
	public override void Start()
	{
		base.Start();
		//GetWeaponStateData("twin");
		//_maxTime = TwinSwordData.coolTime;
	}
	protected override void Skill()
	{
		if (_isCoolTime)
			return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			isSkill = true;
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				SixTimeAttak(Vector3.forward);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				SixTimeAttak(Vector3.back);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				SixTimeAttak(Vector3.left);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				SixTimeAttak(Vector3.right);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && !_isCoolTime)
		{
			_isCoolTime = true;
			SixTimeAttak(Vector3.forward);
		}
	}

	private void SixTimeAttak(Vector3 dir)
	{
		_isCoolTime = true;
		for (int i = 0; i < 6; i++)
		{
			_unitAttack.Attack(dir/*, _basicData.damage, TwinSwordData.freeze*/);
			_unitAttack.onBehaviourEnd = waitReset;
		}
	}

	private void waitReset()
	{
		isSkill = false;
	}
}
