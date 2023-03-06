using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Core;

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
		_playerAnimation.GetClip().SetEventOnFrame(0, () => Attack(vec));
	}
	protected override void Attack(Vector3 vec)
	{
		_playerAttack.AttackColParent.ChangeSizeZ(1);
		_playerAttack.AttackColParent.ChangeSizeX(1);
		_playerAttack.AttackColParent.EnableDir(_playerAttack.AttackColParent.DirReturn(vec));
		_playerAttack.Attack(_unitStat.NowStats.Atk);
		_playerAttack.AttackColParent.AllEnableDir();
		//_playerAttack.AttackColParent.ChangeWeapon();
	}
}
