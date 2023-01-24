using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Managements.Managers;
public class OldStraightSword : BaseStraightSword
{
	public override void Start()
	{
		base.Start();
		_inputManager.ChangeInGameAction(InputTarget.Skill, () => Skill(Vector3.zero));
	}
	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		if (isSkill)
			return;

		if (!_isEnemy)
		{
			_inputManager.ChangeInGameAction(InputTarget.UpMove, () => RollSkill(Vector3.forward * 2));
			_inputManager.ChangeInGameAction(InputTarget.DownMove, () => RollSkill(Vector3.back * 2));
			_inputManager.ChangeInGameAction(InputTarget.LeftMove, () => RollSkill(Vector3.left * 2));
			_inputManager.ChangeInGameAction(InputTarget.RightMove, () => RollSkill(Vector3.right * 2));
		}
		else
			RollSkill(vec);
	}

	private void RollSkill(Vector3 vec)
	{
		isSkill = true;
		_isCoolTime = true;
		UnitMove unitMove = _thisBase.GetBehaviour<UnitMove>();
		unitMove.onBehaviourEnd = RollSkillEnd;
		unitMove.Translate(vec * 2);
	}

	private void RollSkillEnd()
	{
		isSkill = false;

		_inputManager.ChangeInGameAction(InputTarget.UpMove, () => Move(Vector3.forward));
		_inputManager.ChangeInGameAction(InputTarget.DownMove, () => Move(Vector3.back));
		_inputManager.ChangeInGameAction(InputTarget.LeftMove, () => Move(Vector3.left));
		_inputManager.ChangeInGameAction(InputTarget.RightMove, () => Move(Vector3.right));
	}
}
