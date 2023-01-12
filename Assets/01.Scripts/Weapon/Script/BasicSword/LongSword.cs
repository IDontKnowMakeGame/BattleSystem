using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;

public class LongSword : BasicSword
{
	public override void Start()
	{
		base.Start();
		GetWeaponStateData("sword");
		_maxTime = LongSwordData.coolTime;
	}
	protected override void Skill()
	{
		if (_isCoolTime)
			return;

		if (isSkill)
			return;

		if (_inputManager.GetKeyInput(InputManager.InputSignal.Skill))
		{
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveForward))
			{
				RollSkill(Vector3.forward * 2);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveBackward))
			{
				RollSkill(Vector3.back * 2);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveLeft))
			{
				RollSkill(Vector3.left * 2);
			}
			if (_inputManager.GetKeyDownInput(InputManager.InputSignal.MoveRight))
			{
				RollSkill(Vector3.right * 2);
			}
		}

		if (_inputManager.GetKeyUpInput(InputManager.InputSignal.Skill) && !_isCoolTime)
		{
			_isCoolTime = true;
			RollSkill(Vector3.forward * 2);
		}
	}

	private void RollSkill(Vector3 vec)
	{
		isSkill = true;
		_isCoolTime = true;
		UnitMove playerMove = _baseObject.GetBehaviour<UnitMove>();
		playerMove.onBehaviourEnd = RollSkillEnd;
		playerMove.Translate(vec * 2,LongSwordData.skillTime);
	}

	private void RollSkillEnd()
	{
		isSkill = false;
	}
}
