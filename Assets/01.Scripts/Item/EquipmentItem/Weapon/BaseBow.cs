using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBow : Weapon
{
	protected override void ClassLevelSystem()
	{
		_weaponClassLevel = Define.GetManager<DataManager>().LoadWeaponClassLevel("Bow");
		int level = CountToLevel(_weaponClassLevel.killedCount);
		switch (level)
		{
			case 1:
				itemInfo.Atk += 5;
				itemInfo.Ats += 0.5f;
				break;
			case 2:
				itemInfo.Atk += 10;
				itemInfo.Ats += 0.7f;
				break;
			case 3:
				itemInfo.Atk += 15;
				itemInfo.Ats += -0.05f;
				break;
			case 4:
				itemInfo.Atk += 20;
				itemInfo.Ats += -0.07f;
				itemInfo.Afs += -0.01f;
				break;
			case 5:
				itemInfo.Atk += 20;
				itemInfo.Ats += -0.07f;
				itemInfo.Afs += -0.05f;
				break;
			default:
				break;
		};
	}
	public override void UseItem()
	{
		ClassLevelSystem();
		WeaponLevelSystem();

		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);
	}
}
