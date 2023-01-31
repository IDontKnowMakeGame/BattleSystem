using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Managements.Managers;
using Units.Base.Player;
using Units.Base.Unit;
public class OldStraightSword : BaseStraightSword
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldSword");
		_maxTime = OldLongSwordData.coolTime;
	}
	public override void Start()
	{
		base.Start();
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameAction(InputTarget.Skill, InputStatus.Press, () => Skill(Vector3.zero));
	}
	protected override void Skill(Vector3 vec)
	{
		if (_isCoolTime)
			return;

		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
			return;

		RollSkill();
	}

	private void RollSkill()
	{
		_thisBase.AddState(BaseState.Skill);

		_unitMove.distance = 2;
		_unitMove.onBehaviourEnd = RollSkillEnd;
	}

	private void RollSkillEnd()
	{
		_unitMove.distance = 1;
		_isCoolTime = true;
		_thisBase.RemoveState(BaseState.Skill);
	}
}
