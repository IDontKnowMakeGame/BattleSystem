using System.Collections;
using System.Collections.Generic;
using Units.Base.Unit;
using UnityEngine;

public class OldBow : BaseBow
{
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldBow");
	}

	protected override void Skill()
	{
		if (!_thisBase.State.HasFlag(BaseState.Charge))
			return;

		_unitMove.MoveTo(_thisBase.Position + -_currentVector, _unitStat.NowStats.Agi);
	}
}
