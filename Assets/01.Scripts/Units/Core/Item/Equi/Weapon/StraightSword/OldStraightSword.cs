using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
//using Managements.Managers;
public class OldStraightSword : BaseStraightSword
{
	public override void Start()
	{
		base.Start();
		GetWeaponStateData("sword");
		//GetWeaponStateData("sword");
		//_maxTime = LongSwordData.coolTime;
	}
	protected override void Skill()
	{
		if (_isCoolTime)
			return;

		if (isSkill)
			return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				RollSkill(Vector3.forward * 2);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				RollSkill(Vector3.back * 2);
			}
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				RollSkill(Vector3.left * 2);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				RollSkill(Vector3.right * 2);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) && !_isCoolTime)
		{
			_isCoolTime = true;
			RollSkill(Vector3.forward * 2);
		}
	}

	private void RollSkill(Vector3 vec)
	{
		isSkill = true;
		_isCoolTime = true;
		UnitMove unitMove = _thisBase.GetBehaviour<UnitMove>();
		unitMove.onBehaviourEnd = RollSkillEnd;
		unitMove.Translate(vec * 2 /*, speed*/);
	}

	private void RollSkillEnd()
	{
		isSkill = false;
	}
}
