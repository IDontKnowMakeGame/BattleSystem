using UnityEngine;
using Unit.Core.Weapon;
using Managements.Managers;
using Managements;
using Core;
using Units.Base.Player;

public class BaseGreatSword : Weapon
{
	protected float _chargeTime;
	protected float _maxChargeTime => WeaponStat.Ats;

	private Vector3 _currentVector;
	public override void Start()
	{
		base.Start();
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
		LoadClassLevel("GreateSword");
	}
	protected override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);

		if (level == beforeCount)
			return;

		switch(level)
		{
			case 1:
				_weaponStats.Atk += 10;
				_weaponStats.Ats -= 0.01f;
				break;
			case 2:
				_weaponStats.Atk += 15;
				_weaponStats.Ats -= 0.03f;
				break;
			case 3:
				_weaponStats.Atk += 20;
				_weaponStats.Ats -= 0.05f;
				break;
			case 4:
				_weaponStats.Atk += 20;
				_weaponStats.Ats -= 0.07f;
				_weaponStats.Afs -= 0.01f;
				break;
			case 5:
				_weaponStats.Atk += 20;
				_weaponStats.Ats -= 0.07f;
				_weaponStats.Afs -= 0.05f;
				break;
		};
		beforeCount = level;
	}


	public override void Update()
	{
		base.Update();
		//Charge();
	}
	private void AttackVec() => _playerAttack.Attack(_unitStat.NowStats.Atk);
	public override void ChangeKey()
	{
		base.ChangeKey();
		InputManager.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.W);
		InputManager.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.S);
		InputManager.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.A);
		InputManager.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.D);

		InputManager.OnMovePress += Move;
		InputManager.OnAttackHold += Charge;
		InputManager.OnAttackRelease += AttackUP;

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderInit, new EventParam() { floatParam = _maxChargeTime });
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
	}
	private void Move(Vector3 vec)
	{
		if (!_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
		{
			_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_chargeTime = 0;
		}
	}

	protected override void AttackCoroutine(Vector3 vec)
	{
		Attack(vec);
	}

	protected override void Attack(Vector3 vec)
	{
		base.Attack(vec);
		if (_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = true });
		_thisBase.AddState(Units.Base.Unit.BaseState.Charge);
		_thisBase.GetBehaviour<PlayerAttack>().SkillAnimation(vec);
		
		_currentVector = vec;
	}

	private void AttackUP(Vector3 vec)
	{
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		if (_chargeTime >= _maxChargeTime)
		{
			_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_playerAttack.AttackColParent.AllDisableDir();
			_playerAttack.AttackColParent.ChangeSizeZ(1);
			_playerAttack.AttackColParent.ChangeSizeX(1);
			_playerAttack.AttackColParent.EnableDir(_playerAttack.AttackColParent.DirReturn(_currentVector));
			_playerAttack.Attack(_unitStat.NowStats.Atk);
			_playerAttack.AttackColParent.ChangeWeapon();
			_playerAttack.AttackColParent.AllEnableDir();
		}
		else
			InGame.PlayerBase.GetBehaviour<PlayerMove>().stop = false;
		_chargeTime = 0;
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderUp, new EventParam() { floatParam = _chargeTime });
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
	}

	private void Charge(Vector3 vec)
	{
		if (_chargeTime >= _maxChargeTime)
		{
			GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.PullSlider, new EventParam() { color = Color.red });
			return;
		}
		else
		{
			_chargeTime += Time.deltaTime;
			GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderUp, new EventParam() { floatParam = _chargeTime});
		}
	}

	public override void Reset()
	{
		base.Reset();
		_chargeTime = 0;
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		_currentVector = Vector3.zero;
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
		InputManager.OnMovePress -= Move;
		InputManager.OnAttackHold -= Charge;
		InputManager.OnAttackRelease -= AttackUP;
	}
}
