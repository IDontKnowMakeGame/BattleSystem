using System.Collections;
using System;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Core;
using Units.Base.Unit;
using Managements;
public class BaseSpear : Weapon
{
	protected bool _isAttack = false;
	protected Vector3 _currentAttackPos;
	protected bool isOut;

	protected int count = 1;
	public override void Start()
	{
		base.Start();
		LoadClassLevel("Spear");
	}

	public override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);

		switch (level)
		{
			case 1:
				_changeBuffStats.Atk = 5;
				_changeBuffStats.Ats = -0.01f;
				_changeBuffStats.Afs = -0.01f;
				_attackCollider.ChangeSizeZ(1);
				_attackCollider.ChangeSizeX(1);
				break;
			case 2:
				_changeBuffStats.Atk = 10;
				_changeBuffStats.Ats = -0.03f;
				_changeBuffStats.Afs = -0.03f;
				_attackCollider.ChangeSizeZ(1);
				_attackCollider.ChangeSizeX(1);
				break;
			case 3:
				_changeBuffStats.Atk = 15;
				_changeBuffStats.Ats = -0.05f;
				_changeBuffStats.Afs = -0.05f;
				_attackCollider.ChangeSizeZ(1);
				_attackCollider.ChangeSizeX(1);
				break;
			case 4:
				_changeBuffStats.Atk = 20;
				_changeBuffStats.Ats = -0.07f;
				_changeBuffStats.Afs = -0.07f;
				_attackCollider.ChangeSizeZ(1);
				_attackCollider.ChangeSizeX(1);
				break;
			case 5:
				_changeBuffStats.Atk = 20;
				_changeBuffStats.Ats = -0.07f;
				_changeBuffStats.Afs = -0.07f;
				int sizeZ = _currentAttackPos == Vector3.forward || _currentAttackPos == Vector3.back ? 2 : 1;
				int sizeX = _currentAttackPos == Vector3.left || _currentAttackPos == Vector3.right ? 2 : 1;
				_attackCollider.ChangeSizeZ(sizeZ);
				_attackCollider.ChangeSizeX(sizeX);
				break;
		};
	}
	protected override void Attack()
	{
		base.Attack();
		IsOut();
	}
	public override void ChangeKey()
	{
		base.ChangeKey();
		Debug.Log("SpearChange");
		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);
	}
	private void IsOut() => isOut = false;
	public override void Update()
	{
		base.Update();
		if (_isAttack && isOut && /*_playerAttack.HasEnemy() &&*/ !_thisBase.State.HasFlag(BaseState.Moving))
		{
			Attack();
			_attackCollider.ChangeWeapon();
		}
		else if (_isAttack /*&& ! _playerAttack.HasEnemy()*/)
		{
			isOut = true;
		}

		if (_isAttack)
			RangeOn();
	}

	protected override void AttackCoroutine(Vector3 vec)
	{
		Attack(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		base.Attack(vec);
		if (!_isAttack)
		{
			_attackCollider.AllDisableDir();
			_currentAttackPos = Vector3.zero;
			_isAttack = true;
			_thisBase.StartCoroutine(DownLate(vec));
		}

		if (_isAttack && _currentAttackPos == vec)
		{
			_thisBase.RemoveState(BaseState.Attacking);
			_attackCollider.AllDisableDir();
			_isAttack = false;
			count = 0;
		}
	}

	private IEnumerator DownLate(Vector3 vec)
	{
		yield return new WaitForSeconds(WeaponStat.Ats);
		_currentAttackPos = vec;
		LevelSystem();
		_attackCollider.CheckDir(_attackCollider.DirReturn(_currentAttackPos));
		Attack();
	}

	private void RangeOn()
	{
		GameManagement.Instance.GetManager<MapManager>().RangeOn(_thisBase.Position + _currentAttackPos, Color.green);
	}
	public override void Reset()
	{
		base.Reset();
		isOut = false;
		_currentAttackPos = Vector3.zero;
		_isAttack = false;
		_attackCollider.AllDisableDir();
	}
}
