using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
public class BaseTwinSword : Weapon
{
	public override void Start()
	{
		base.Start();
		if (!_isEnemy)
		{
			_inputManager.ChangeInGameKey(InputTarget.UpMove, KeyCode.UpArrow);
			_inputManager.ChangeInGameKey(InputTarget.DownMove, KeyCode.DownArrow);
			_inputManager.ChangeInGameKey(InputTarget.LeftMove, KeyCode.LeftArrow);
			_inputManager.ChangeInGameKey(InputTarget.RightMove, KeyCode.RightArrow);

			_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.UpArrow);
			_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.DownArrow);
			_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.LeftArrow);
			_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.RightArrow);
		}
	}

	protected override void Move(Vector3 vec)
	{
		if (isSkill)
			return;

		_unitMove.Translate(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			_unitAttack.Attack(vec + Vector3.left);
			_unitAttack.Attack(vec + Vector3.right);

			// _attackCollider.AllDisableDir();
			// DirType dirType = vec == Vector3.forward ? DirType.Up : DirType.Down;
			// _attackCollider.ChangeSizeZ(dirType,2);
			// _attackCollider.EnableDir(DirType.Left, DirType.Right);
		}
		else
		{
			_unitAttack.Attack(vec + Vector3.forward);
			_unitAttack.Attack(vec + Vector3.back);

			// _attackCollider.AllDisableDir();
			// DirType dirType = vec == Vector3.forward ? DirType.Up : DirType.Down;
			// _attackCollider.ChangeSizeX(dirType,2);
			// _attackCollider.EnableDir(DirType.Up, DirType.Down);
		}
	}
}
