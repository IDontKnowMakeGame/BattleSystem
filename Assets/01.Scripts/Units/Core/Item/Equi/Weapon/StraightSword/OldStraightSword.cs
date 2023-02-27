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
		_maxTime = OldLongSwordData.coolTime;
	}
	public override void Start()
	{
		base.Start();
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
	}
	protected override void Skill()
	{
		if (_isCoolTime)
			return;

		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
			return;


		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Moving))
			return;

		RollSkill();
	}

	private void RollSkill()
	{
		_thisBase.AddState(BaseState.Skill);

		_unitMove.distance = 2;
		_thisBase.GetBehaviour<PlayerAnimation>().CurWeaponAnimator.Skill = true;
		_unitMove.onBehaviourEnd = RollSkillEnd;
	}

	private void RollSkillEnd()
	{
		_unitMove.distance = 1;
		_isCoolTime = true;
		_thisBase.RemoveState(BaseState.Skill);
	}
}
