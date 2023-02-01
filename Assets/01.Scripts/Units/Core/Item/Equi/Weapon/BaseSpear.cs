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
	public override void ChangeKey()
	{
		base.ChangeKey();

		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_playerAttack.onBehaviourEnd = IsOut;
	}
	private void IsOut() => isOut = false;
	public override void Update()
	{
		base.Update();
		Debug.Log(_playerAttack.HasEnemy());
		if (_isAttack && isOut && _playerAttack.HasEnemy() && !_thisBase.State.HasFlag(BaseState.Moving))
		{
			_playerAttack.Attack(_unitStat.NowStats.Atk);
			_playerAttack.AttackColParent.ChangeWeapon();
		}
		else if (_isAttack && ! _playerAttack.HasEnemy())
		{
			isOut = true;
		}

		if (_isAttack)
			RangeOn();
	}

	protected override void Attack(Vector3 vec)
	{

		base.Attack(vec);
		if (!_isAttack)
		{
			_playerAttack.AttackColParent.AllDisableDir();
			_currentAttackPos = Vector3.zero;
			_isAttack = true;
			_thisBase.StartCoroutine(DownLate(vec));
		}

		if (_isAttack && _currentAttackPos == vec)
		{
			_playerAttack.AttackColParent.AllDisableDir();
			_isAttack = false;
			count = 0;
		}
	}

	private IEnumerator DownLate(Vector3 vec)
	{
		yield return new WaitForSeconds(_weaponStats.Ats);
		_currentAttackPos = vec;
		_playerAttack.AttackColParent.ChangeSizeZ(1);
		_playerAttack.AttackColParent.ChangeSizeX(1);
		_playerAttack.AttackColParent.EnableDir(_playerAttack.AttackColParent.DirReturn(_currentAttackPos));
		_playerAttack.Attack(_unitStat.NowStats.Atk);
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
		_playerAttack.AttackColParent.AllDisableDir();
		_playerAttack.onBehaviourEnd = null;
	}
}
