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
		_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.UpArrow);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.DownArrow);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.LeftArrow);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.RightArrow);
	}

	protected override void Attack(Vector3 vec)
	{
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			_playerAttack.AttackColParent.AllDisableDir();
			_playerAttack.AttackColParent.ChangeSizeZ(2);
			_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
			_unitAttack.Attack(vec + Vector3.left);
			_unitAttack.Attack(vec + Vector3.right);
			Debug.Log("twinSword" + vec);
		}
		else
		{
			_playerAttack.AttackColParent.AllDisableDir();
			_playerAttack.AttackColParent.ChangeSizeZ(2);
			_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
			_unitAttack.Attack(vec + Vector3.forward);
			_unitAttack.Attack(vec + Vector3.back);
			Debug.Log("twinSword" + vec);
		}
	}
}
