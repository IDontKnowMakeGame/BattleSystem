using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
public class TaintedSword : BaseStraightSword
{
	public override void Awake()
	{
		base.Awake();

	}

	public override void ChangeKey()
	{
		base.ChangeKey();
	}
	protected override void Skill()
	{
		if (_isCoolTime)
			return;

		if (thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
			return;

		SoulRealm();
	}

	private void SoulRealm()
	{
		thisBase.AddState(Units.Base.Unit.BaseState.Skill);
		thisBase.AddState(Units.Base.Unit.BaseState.StopMove);
		float atk = WeaponStat.Atk;
		WeaponStat.Atk = atk * 1.5f;
		Debug.Log(">");
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.forward, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.forward + Vector3.left, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.forward + Vector3.right, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.back, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.back + Vector3.left, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.back + Vector3.right, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.left, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(thisBase.Position + Vector3.right, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
	}

	private void RealmEnd(float atk)
	{
		_weaponStats.Atk = atk;
		thisBase.RemoveState(Units.Base.Unit.BaseState.Skill);
		thisBase.RemoveState(Units.Base.Unit.BaseState.StopMove);
	}
}
