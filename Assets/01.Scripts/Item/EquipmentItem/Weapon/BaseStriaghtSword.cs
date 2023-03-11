using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStriaghtSword : Weapon
{
	WeaponClassLevel _weaponClassLevel;
	protected override void ClassLevelSystem()
	{
		_weaponClassLevel = Define.GetManager<DataManager>().LoadWeaponClassLevel(this.GetType().ToString());
		int level = CountToLevel(_weaponClassLevel.killedCount);
		Debug.Log(level);
		switch (level)
		{
			case 1:
				itemInfo.Atk = 10;
				itemInfo.Ats = -0.01f;
				break;
			case 2:
				itemInfo.Atk = 15;
				itemInfo.Ats = -0.03f;
				break;
			case 3:
				itemInfo.Atk = 20;
				itemInfo.Ats = -0.05f;
				break;
			case 4:
				itemInfo.Atk = 20;
				itemInfo.Ats = -0.07f;
				itemInfo.Afs = -0.01f;
				break;
			case 5:
				itemInfo.Atk = 20;
				itemInfo.Ats = -0.07f;
				itemInfo.Afs = -0.05f;
				break;
			default:
				break;
		};
	}
	public override void UseItem()
	{
		ClassLevelSystem();
		WeaponLevelSystem();
		
		Debug.Log("?");
		//_attackCollider.ChangeSizeZ(1);
		//_attackCollider.ChangeSizeX(1);
		//_attackCollider.CheckDir(_attackCollider.DirReturn(vec));
		//_attackCollider.AllEnableDir();

		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);
	}
}
