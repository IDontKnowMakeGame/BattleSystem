using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;
using Units.Base.Unit;
public class OldTwinSword : BaseTwinSword
{
	float _freezeTime;
	public override void Awake()
	{
		base.Awake();
		GetWeaponStateData("oldTwinSword");
		_maxTime = OldTwinSwordData.coolTime;
		_freezeTime = OldTwinSwordData.freeze;
	}

	public override void ChangeKey()
	{
		base.ChangeKey();
	}
	protected override void Skill()
	{
		if (_thisBase.State.HasFlag(BaseState.Skill))
			return;

		_thisBase.AddState(BaseState.Skill);
		_thisBase.AddState(BaseState.StopMove);

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Press, AttackFoword);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Press, AttackBack);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Press, AttackLeft);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Press, AttackRight);
	}

	protected override void Attack(Vector3 vec)
	{
		if (_thisBase.State.HasFlag(BaseState.Skill))
			return;
		base.Attack(vec);
	}

	private void SixTimeAttak(Vector3 dir)
	{
		for (int i = 0; i < 6; i++)
		{
			Define.GetManager<MapManager>().Damage(_thisBase.Position+dir,_unitStat.NowStats.Atk, _freezeTime, waitReset, InGame.PlayerBase);
		}
	}

	private void AttackFoword() => SixTimeAttak(Vector3.forward);
	private void AttackBack() => SixTimeAttak(Vector3.back);
	private void AttackLeft() => SixTimeAttak(Vector3.left);
	private void AttackRight() => SixTimeAttak(Vector3.right);

	private void waitReset()
	{
		_isCoolTime = true;
		_thisBase.RemoveState(BaseState.Skill);
		_thisBase.RemoveState(BaseState.StopMove);

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.RightArrow);

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Press, AttackFoword);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Press, AttackBack);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Press, AttackLeft);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Press, AttackRight);
	}

	public override void Reset()
	{
		base.Reset();
	}
}
