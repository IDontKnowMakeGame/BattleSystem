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
		GetWeaponStateData("taintedSword");
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

		SoulRealm();
	}

	private void SoulRealm()
	{
		_thisBase.AddState(Units.Base.Unit.BaseState.Skill);
		_thisBase.AddState(Units.Base.Unit.BaseState.StopMove);
		float atk = _weaponStats.Atk;
		_weaponStats.Atk = atk * 1.5f;
		Debug.Log(">");
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.forward, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.forward + Vector3.left, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.forward + Vector3.right, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.back, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.back + Vector3.left, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.back + Vector3.right, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.left, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
		Define.GetManager<MapManager>().Damage(_thisBase.Position + Vector3.right, _unitStat.NowStats.Atk, 0.5f, () => RealmEnd(atk));
	}

	private void RealmEnd(float atk)
	{
		_weaponStats.Atk = atk;
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Skill);
		_thisBase.RemoveState(Units.Base.Unit.BaseState.StopMove);
	}
}
