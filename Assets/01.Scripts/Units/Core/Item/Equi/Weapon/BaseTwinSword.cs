using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Units.Base.Player;
public class BaseTwinSword : Weapon
{
	public override void Start()
	{
		base.Start();
		LoadClassLevel("TwinSword");
	}
	protected override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);

		switch (level)
		{
			case 1:
				_changeWeaponStats.Atk = 5;
				break;
			case 2:
				_changeWeaponStats.Atk = 10;
				break;
			case 3:
				_changeWeaponStats.Atk = 10;
				break;
			case 4:
				_changeWeaponStats.Atk = 15;
				break;
			case 5:
				_changeWeaponStats.Atk = 20;
				break;
		};
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		Debug.Log("ChangeTwinSword");
		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.UpArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.DownArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.LeftArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.RightArrow);
	}

	protected override void AttackCoroutine(Vector3 vec)
	{
		_unitMove.onBehaviourEnd = () => Attack(vec);
	}
	protected override void Attack(Vector3 vec)
	{
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			_playerAttack.AttackColParent.AllDisableDir();
			_playerAttack.AttackColParent.ChangeSizeZ(DirType.Left,1);
			_playerAttack.AttackColParent.ChangeSizeZ(DirType.Right, 1);
			_playerAttack.AttackColParent.ChangeSizeX(1);
			_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
			_playerAttack.Attack(_unitStat.NowStats.Atk);
			_playerAttack.AttackColParent.ChangeWeapon();
			_playerAttack.AttackColParent.AllEnableDir();
		}
		else
		{
			_playerAttack.AttackColParent.AllDisableDir();
			_playerAttack.AttackColParent.ChangeSizeX(DirType.Up, 1);
			_playerAttack.AttackColParent.ChangeSizeX(DirType.Down, 1);
			_playerAttack.AttackColParent.ChangeSizeZ(1);
			_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
			_playerAttack.Attack(_unitStat.NowStats.Atk);
			_playerAttack.AttackColParent.ChangeWeapon();
			_playerAttack.AttackColParent.AllEnableDir();
		}
	}

	public override void Reset()
	{
		_unitMove.onBehaviourEnd = null;
		base.Reset();
	}
}
