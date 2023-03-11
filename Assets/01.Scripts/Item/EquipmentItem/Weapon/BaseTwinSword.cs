using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTwinSword : Weapon
{
	protected override void ClassLevelSystem()
	{
		_weaponClassLevel = Define.GetManager<DataManager>().LoadWeaponClassLevel("Twin");
		int level = CountToLevel(_weaponClassLevel.killedCount);
		switch (level)
		{
			case 1:
				itemInfo.Atk += 5;
				break;
			case 2:
				itemInfo.Atk += 10;
				break;
			case 3:
				itemInfo.Atk += 10;
				_attackInfo.SizeX = 2;
				_attackInfo.SizeZ = 2;
				break;
			case 4:
				itemInfo.Atk += 15;
				_attackInfo.SizeX = 2;
				_attackInfo.SizeZ = 2;
				break;
			case 5:
				itemInfo.Atk += 20;
				_attackInfo.SizeX = 3;
				_attackInfo.SizeZ = 3;
				break;
			default:
				break;
		};
	}
	public override void UseItem()
	{
		ClassLevelSystem();
		WeaponLevelSystem();

		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.UpArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.DownArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.LeftArrow);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.RightArrow);
	}
}
