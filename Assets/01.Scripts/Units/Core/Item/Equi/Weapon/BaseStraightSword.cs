using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;

public class BaseStraightSword : Weapon
{
	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);
	}

	protected override void Attack(Vector3 vec)
	{
		_playerAttack.AttackColParent.AllDisableDir();
		_playerAttack.AttackColParent.ChangeSizeZ(1);
		_playerAttack.AttackColParent.ChangeSizeX(1);
		_playerAttack.AttackColParent.EnableDir(_playerAttack.AttackColParent.DirReturn(vec));
		_playerAttack.Attack(_unitStat.NowStats.Atk);
		_playerAttack.AttackColParent.AllEnableDir();
		_playerAttack.AttackColParent.ChangeWeapon();
	}

	public override void Reset()
	{
		base.Reset();
	}
}
