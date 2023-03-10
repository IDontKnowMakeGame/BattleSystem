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
		_maxTime = OldTwinSwordData.coolTime;
		_freezeTime = OldTwinSwordData.freeze;
	}

	public override void ChangeKey()
	{
		base.ChangeKey();
	}
	protected override void Skill()
	{
		if (thisBase.State.HasFlag(BaseState.Skill))
			return;

		thisBase.AddState(BaseState.Skill);
		thisBase.AddState(BaseState.StopMove);

		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);


		InputManager.OnAttackPress += SixTimeAttak;
	}

	protected override void Attack(Vector3 vec)
	{
		if (thisBase.State.HasFlag(BaseState.Skill))
			return;
		base.Attack(vec);
	}

	private void SixTimeAttak(Vector3 dir)
	{
		for (int i = 0; i < 6; i++)
		{
			Define.GetManager<MapManager>().Damage(thisBase.Position+dir,_unitStat.NowStats.Atk, _freezeTime, waitReset, InGame.PlayerBase);
		}
	}

	private void waitReset()
	{
		_isCoolTime = true;
		thisBase.RemoveState(BaseState.Skill);
		thisBase.RemoveState(BaseState.StopMove);

		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.UpArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.DownArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.LeftArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.RightArrow);

		InputManager.OnAttackPress -= SixTimeAttak;
	}

	public override void Reset()
	{
		base.Reset();
	}
}
