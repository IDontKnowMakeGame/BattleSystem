using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Units.Base.Unit;
public class OldTwinSword : BaseTwinSword
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldTwinSword");
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

		_thisBase.AddState(BaseState.Skill);
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
		_thisBase.RemoveState(BaseState.Skill);
	}
}
