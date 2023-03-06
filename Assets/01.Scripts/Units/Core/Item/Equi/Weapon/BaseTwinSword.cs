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
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Left, 1);
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Right, 1);
					_playerAttack.AttackColParent.ChangeSizeX(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					Debug.Log("옆이용");
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Up, 1);
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Down, 1);
					_playerAttack.AttackColParent.ChangeSizeZ(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 2:
				_changeBuffStats.Atk = 10;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Left, 1);
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Right, 1);
					_playerAttack.AttackColParent.ChangeSizeX(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Up, 1);
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Down, 1);
					_playerAttack.AttackColParent.ChangeSizeZ(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 3:
				_changeBuffStats.Atk = 10;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Left, 2);
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Right, 2);
					_playerAttack.AttackColParent.ChangeSizeX(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Up, 2);
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Down, 2);
					_playerAttack.AttackColParent.ChangeSizeZ(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 4:
				_changeBuffStats.Atk = 15;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Left, 2);
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Right, 2);
					_playerAttack.AttackColParent.ChangeSizeX(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
				}
				else
				{
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Up, 2);
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Down, 2);
					_playerAttack.AttackColParent.ChangeSizeZ(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
				}
				break;
			case 5:
				_changeBuffStats.Atk = 20;
				if (_currentVec == Vector3.forward || _currentVec == Vector3.back)
				{
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Left, 2);
					_playerAttack.AttackColParent.ChangeSizeZ(DirType.Right, 2);
					_playerAttack.AttackColParent.ChangeSizeX(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Left, DirType.Right);
					DirType type = _currentVec == Vector3.forward ? DirType.Up : DirType.Down;
					_playerAttack.AttackColParent.EnableDir(type);
					float offset = _currentVec == Vector3.forward ? 0.5f : -0.5f;
					_playerAttack.AttackColParent.ChangeOffsetZ(DirType.Left, offset);
					_playerAttack.AttackColParent.ChangeOffsetZ(DirType.Right, offset);
				}
				else
				{
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Up, 2);
					_playerAttack.AttackColParent.ChangeSizeX(DirType.Down, 2);
					_playerAttack.AttackColParent.ChangeSizeZ(1);
					_playerAttack.AttackColParent.EnableDir(DirType.Up, DirType.Down);
					DirType type = _currentVec == Vector3.left ? DirType.Left : DirType.Right;
					_playerAttack.AttackColParent.EnableDir(type);
					float offset = _currentVec == Vector3.right ? 0.5f : -0.5f;
					_playerAttack.AttackColParent.ChangeOffsetX(DirType.Up, offset);
					_playerAttack.AttackColParent.ChangeOffsetX(DirType.Down, offset);
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
		_playerAttack.AttackColParent.AllDisableDir();
		LevelSystem();
		_playerAttack.Attack(_unitStat.NowStats.Atk);
		_playerAttack.AttackColParent.ChangeWeapon();
		_playerAttack.AttackColParent.AllEnableDir();
	}

	public override void Reset()
	{
		_unitMove.onBehaviourEnd = null;
		base.Reset();
	}
}
