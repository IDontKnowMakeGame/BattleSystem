using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
public class OldGreatSword : BaseGreatSword
{
	private float godTime;
	public override void Awake()
	{
		base.Awake();
		_maxTime = OldGreatSwordData.coolTime;
		godTime = OldGreatSwordData.gt;
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
		if (_isCoolTime || _thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
			return;

		_thisBase.StartCoroutine(HoldOn());
	}

	private IEnumerator HoldOn()
	{
		_thisBase.AddState(Units.Base.Unit.BaseState.Skill);
		_unitStat.Half += 30;
		yield return new WaitForSeconds(godTime);
		_unitStat.Half = 0;
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Skill);
		_isCoolTime = true;
	}
}
