using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Core;
using Units.Base.Player;
using Units.Base.Unit;

public class BaseStraightSword : Weapon
{
	public override void Start()
	{
		base.Start();
		LoadClassLevel("BasicSword");
		LevelSystem();
	}
	public override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);
		switch (level)
		{
			case 1:
				_changeBuffStats.Atk = 10;
				_changeBuffStats.Ats = -0.01f;
				break;
			case 2:
				_changeBuffStats.Atk = 15;
				_changeBuffStats.Ats = -0.03f;
				break;
			case 3:
				_changeBuffStats.Atk = 20;
				_changeBuffStats.Ats = -0.05f;
				break;
			case 4:
				_changeBuffStats.Atk = 20;
				_changeBuffStats.Ats = -0.07f;
				_changeBuffStats.Afs = -0.01f;
				break;
			case 5:
				_changeBuffStats.Atk = 20;
				_changeBuffStats.Ats = -0.07f;
				_changeBuffStats.Afs = -0.05f;
				break;
			default:
				break;
		};
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);
	}
	protected override void AttackCoroutine(Vector3 vec)
	{
		base.AttackCoroutine(vec);
		if(thisBase.State.HasFlag(BaseState.Attacking))
			_playerAnimation.GetClip().SetEventOnFrame(5, () => Attack(vec));
	}
	protected override void Attack(Vector3 vec)
	{
		_attackCollider.ChangeSizeZ(1);
		_attackCollider.ChangeSizeX(1);
		_attackCollider.CheckDir(_attackCollider.DirReturn(vec));
		Attack();
		_attackCollider.AllEnableDir();
	}
}
