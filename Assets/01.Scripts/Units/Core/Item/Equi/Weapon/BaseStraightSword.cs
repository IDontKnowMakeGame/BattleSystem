using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Core;

public class BaseStraightSword : Weapon
{
	public override void ChangeKey()
	{
		base.ChangeKey();
		_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

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
	}
}
