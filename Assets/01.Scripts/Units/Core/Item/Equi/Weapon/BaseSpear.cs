using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Core;
public class BaseSpear : Weapon
{
	protected bool _isAttack;
	protected Vector3 _currentAttackPos;
	protected int count;
	protected bool isOut;
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

	public override void Update()
	{
		base.Update();
		if (_isAttack && InGame.GetUnit(_thisBase.Position + _currentAttackPos) && isOut)
		{
			isOut = false;
			count++;
			_unitAttack.Attack(_currentAttackPos);
		}
		else if(_isAttack)
		{
			isOut = true;
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
		if(!_isAttack)
		{
			_currentAttackPos = Vector3.zero;
			_isAttack = true;
			_thisBase.StartCoroutine(DownLate(vec));
		}

		if (_isAttack && _currentAttackPos == vec)
			_isAttack = false;
	}

	private IEnumerator DownLate(Vector3 vec)
	{
		yield return new WaitForSeconds(_weaponStats.Ats);
		_currentAttackPos = vec;
	}
}
