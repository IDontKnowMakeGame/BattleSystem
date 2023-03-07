using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Units.Base.Player;
public class BaseTwinSword : Weapon
{
	Vector3 _currentVec = Vector3.zero;
	public override void Start()
	{
		base.Start();
		LoadClassLevel("TwinSword");
	}
	public override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);

		Debug.Log(level);
		switch (level)
		{
			case 1:
				_changeBuffStats.Atk = 5;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					Debug.Log("앞이용");
					_attackCollider.ChangeSizeZ(DirType.Left, 1);
					_attackCollider.ChangeSizeZ(DirType.Right, 1);
					_attackCollider.ChangeSizeX(1);
					_attackCollider.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					Debug.Log("옆이용");
					_attackCollider.ChangeSizeX(DirType.Up, 1);
					_attackCollider.ChangeSizeX(DirType.Down, 1);
					_attackCollider.ChangeSizeZ(1);
					_attackCollider.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 2:
				_changeBuffStats.Atk = 10;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_attackCollider.ChangeSizeZ(DirType.Left, 1);
					_attackCollider.ChangeSizeZ(DirType.Right, 1);
					_attackCollider.ChangeSizeX(1);
					_attackCollider.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					_attackCollider.ChangeSizeX(DirType.Up, 1);
					_attackCollider.ChangeSizeX(DirType.Down, 1);
					_attackCollider.ChangeSizeZ(1);
					_attackCollider.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 3:
				_changeBuffStats.Atk = 10;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_attackCollider.ChangeSizeZ(DirType.Left, 2);
					_attackCollider.ChangeSizeZ(DirType.Right, 2);
					_attackCollider.ChangeSizeX(1);
					_attackCollider.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					_attackCollider.ChangeSizeX(DirType.Up, 2);
					_attackCollider.ChangeSizeX(DirType.Down, 2);
					_attackCollider.ChangeSizeZ(1);
					_attackCollider.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 4:
				_changeBuffStats.Atk = 15;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_attackCollider.ChangeSizeZ(DirType.Left, 2);
					_attackCollider.ChangeSizeZ(DirType.Right, 2);
					_attackCollider.ChangeSizeX(1);
					_attackCollider.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					_attackCollider.ChangeSizeX(DirType.Up, 2);
					_attackCollider.ChangeSizeX(DirType.Down, 2);
					_attackCollider.ChangeSizeZ(1);
					_attackCollider.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 5:
				_changeBuffStats.Atk = 20;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_attackCollider.ChangeSizeZ(DirType.Left, 2);
					_attackCollider.ChangeSizeZ(DirType.Right, 2);
					_attackCollider.ChangeSizeX(1);
					_attackCollider.EnableDir(DirType.Left, DirType.Right);
					DirType type = _currentVec == Vector3.forward ? DirType.Up : DirType.Down;
					_attackCollider.CheckDir(type);
					float offset = _currentVec == Vector3.forward ? 0.5f : -0.5f;
					_attackCollider.ChangeOffsetZ(DirType.Left, offset);
					_attackCollider.ChangeOffsetZ(DirType.Right, offset);
				}
				else
				{
					_attackCollider.ChangeSizeX(DirType.Up, 2);
					_attackCollider.ChangeSizeX(DirType.Down, 2);
					_attackCollider.ChangeSizeZ(1);
					_attackCollider.EnableDir(DirType.Up, DirType.Down);
					DirType type = _currentVec == Vector3.left ? DirType.Left : DirType.Right;
					_attackCollider.CheckDir(type);
					float offset = _currentVec == Vector3.right ? 0.5f : -0.5f;
					_attackCollider.ChangeOffsetX(DirType.Up, offset);
					_attackCollider.ChangeOffsetX(DirType.Down, offset);
				}
				break;
		};
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
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
		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Moving))
			return;

		_currentVec = vec;
		_attackCollider.AllDisableDir();
		LevelSystem();
		Attack();
		_attackCollider.ChangeWeapon();
		_attackCollider.AllEnableDir();
	}

	public override void Reset()
	{
		_unitMove.onBehaviourEnd = null;
		base.Reset();
	}
}
