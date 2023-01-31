using System.Collections;
using System;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Core;
using Units.Base.Unit;
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
	}

	public override void Update()
	{
		base.Update();
		//Debug.Log(InGame.GetUnit(_thisBase.Position + _currentAttackPos));
		//Debug.Log(isOut);
		//Debug.Log(_isAttack);
		if (_isAttack && InGame.GetUnit(_thisBase.Position + _currentAttackPos) != null && isOut && !_thisBase.State.HasFlag(BaseState.Moving))
		{
			Debug.Log("������");
			isOut = false;
			_playerAttack.Attack(_unitStat.NowStats.Atk);
		}
		else if(_isAttack && !InGame.GetUnit(_thisBase.Position + _currentAttackPos))
		{
			isOut = true;
		}
	}

	protected override void Attack(Vector3 vec)
	{
		if(!_isAttack)
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
	public override void Reset()
	{
		isOut = false;
		_currentAttackPos = Vector3.zero;
		_isAttack = false;
		_playerAttack.AttackColParent.AllDisableDir();
		_playerAttack.onBehaviourEnd = null;
	}
}
