using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Managements.Managers;
using Units.Base.Player;

public class OldStraightSword : BaseStraightSword
{
	public override void Start()
	{
		base.Start();
		GetWeaponStateData("oldSword");
		_inputManager.ChangeInGameAction(InputTarget.Skill, InputStatus.Press, () => Skill(Vector3.zero));
	}
	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		if (isSkill)
			return;

		if (!_isEnemy)
		{
			_inputManager.ChangeInGameAction(InputTarget.UpMove, InputStatus.Press,() => RollSkill(Vector3.forward * 2));
			_inputManager.ChangeInGameAction(InputTarget.DownMove, InputStatus.Press, () => RollSkill(Vector3.back * 2));
			_inputManager.ChangeInGameAction(InputTarget.LeftMove, InputStatus.Press, () => RollSkill(Vector3.left * 2));
			_inputManager.ChangeInGameAction(InputTarget.RightMove, InputStatus.Press, () => RollSkill(Vector3.right * 2));
		}
		else
			RollSkill(vec);
	}

	private void RollSkill(Vector3 vec)
	{
		
		Debug.Log(1);
		isSkill = true;
		_isCoolTime = true;
		PlayerMove unitMove = _thisBase.GetBehaviour<PlayerMove>();
		unitMove.onBehaviourEnd = RollSkillEnd;
		unitMove.Translate(vec * 2);
	}

	private void RollSkillEnd()
	{
		Debug.Log(2);
		isSkill = false;

		_inputManager.ChangeInGameAction(InputTarget.UpMove, InputStatus.Press, () =>
		{
			Move(Vector3.forward);
		});
		_inputManager.ChangeInGameAction(InputTarget.DownMove, InputStatus.Press, () => Move(Vector3.back));
		_inputManager.ChangeInGameAction(InputTarget.LeftMove, InputStatus.Press, () => Move(Vector3.left));
		_inputManager.ChangeInGameAction(InputTarget.RightMove, InputStatus.Press, () => Move(Vector3.right));
	}
}
