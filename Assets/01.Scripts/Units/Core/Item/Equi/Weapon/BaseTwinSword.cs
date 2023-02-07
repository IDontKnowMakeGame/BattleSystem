using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Units.Base.Player;
public class BaseTwinSword : Weapon
{
	public override void ChangeKey()
	{
		base.ChangeKey();
		Debug.Log("ChangeTwinSword");
		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.RightArrow);

	}

	protected override void Attack(Vector3 vec)
	{
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			Debug.Log("�յ�" + vec);
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
			Debug.Log("����" + vec);
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
		Debug.Log("Twin");
		base.Reset();
	}
}
