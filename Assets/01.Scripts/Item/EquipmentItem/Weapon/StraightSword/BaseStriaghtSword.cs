using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseStriaghtSword : Weapon
{
	//protected override void ClassLevelSystem()
	//{
	//	int level = CountToLevel(_weaponClassLevel.killedCount);
	//	switch (level)
	//	{
	//		case 1:
	//			_changeBuffStats.Atk = 10;
	//			_changeBuffStats.Ats = -0.01f;
	//			break;
	//		case 2:
	//			_changeBuffStats.Atk = 15;
	//			_changeBuffStats.Ats = -0.03f;
	//			break;
	//		case 3:
	//			_changeBuffStats.Atk = 20;
	//			_changeBuffStats.Ats = -0.05f;
	//			break;
	//		case 4:
	//			_changeBuffStats.Atk = 20;
	//			_changeBuffStats.Ats = -0.07f;
	//			_changeBuffStats.Afs = -0.01f;
	//			break;
	//		case 5:
	//			_changeBuffStats.Atk = 20;
	//			_changeBuffStats.Ats = -0.07f;
	//			_changeBuffStats.Afs = -0.05f;
	//			break;
	//		default:
	//			break;
	//	};
	//}
	public override void UseItem()
	{
		//_attackCollider.ChangeSizeZ(1);
		//_attackCollider.ChangeSizeX(1);
		//_attackCollider.CheckDir(_attackCollider.DirReturn(vec));
		//_attackCollider.AllEnableDir();
		//LevelSystem();

		//InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		//InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		//InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		//InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);
	}
}
