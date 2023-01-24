using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Unit.Core.Weapon;
public class BaseBow : Weapon
{
	private bool _isCharge;
	protected float _chargeTime;
	protected float _maxChargeTime;

	private Vector3 _currentVector;
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

	public override void Update()
	{
		base.Update();
		Charge();
	}

	protected override void Move(Vector3 vec)
	{
		if (isSkill)
			return;
		_isCharge = false;

		_unitMove.Translate(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		_isCharge = true;
		_currentVector = vec;
	}

	private void Charge()
	{
		if (!_isCharge)
			return;

		if (_chargeTime >= _maxChargeTime)
		{
			_isCharge = false;
			_unitAttack.Attack(_currentVector);
		}
		else
		{
			_chargeTime += Time.deltaTime;
		}
	}
}
