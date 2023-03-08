using System.Collections;
using System.Collections.Generic;
using Units.Base.Unit;
using UnityEngine;

public class OldBow : BaseBow
{
	protected override void Skill()
	{
		if (!thisBase.State.HasFlag(BaseState.Charge))
			return;

		_unitMove.MoveTo(thisBase.Position + -_currentVector, _unitStat.NowStats.Agi);
	}
}
