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
		LevelSystem();
	}
	public override void LevelSystem()
	{
		int level = CountToLevel(_weaponClassLevel.killedCount);

		switch (level)
		{
			case 1:
				_unitStat.Half = 5;
				_unitStat.onBehaviourEnd = () => _unitStat.Half = 0;
				break;
			case 2:
				_unitStat.Half = 10;
				_unitStat.onBehaviourEnd = () => _unitStat.Half = 0;
				break;
			case 3:
				_unitStat.Half = 15;
				_unitStat.onBehaviourEnd = () => _unitStat.Half = 0;
				break;
			case 4:
				_unitStat.Half = 20;
				_unitStat.onBehaviourEnd = () => _unitStat.Half = 0;
				break;
			case 5:
				_unitStat.Half = 20;
				_unitStat.onBehaviourEnd = () => _unitStat.Half = 0;
				_changeBuffStats.Atk = 20;
				break;
		};
	}
	public override void Update()
	{
		base.Update();
		//Charge();
	}
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
		if (!thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
		{
			thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
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
		if (thisBase.State.HasFlag(Units.Base.Unit.BaseState.Charge))
			return;

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = true });
		thisBase.AddState(Units.Base.Unit.BaseState.Charge);

		thisBase.GetBehaviour<PlayerAttack>().ChargeAnimation(vec);
		
		_currentVector = vec;

		LevelSystem();
	}

	private void AttackUP(Vector3 vec)
	{
		thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		if (_chargeTime >= _maxChargeTime)
		{
			LevelSystem();
			thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_attackCollider.ChangeSizeZ(1);
			_attackCollider.ChangeSizeX(1);
			_attackCollider.CheckDir(_attackCollider.DirReturn(_currentVector));
			_playerAnimation.CurWeaponAnimator.SetDir = vec;
			_playerAnimation.CurWeaponAnimator.Attack = true;
			Attack();
			_playerAnimation.SetAnmation();
			_attackCollider.AllEnableDir();
		}
		else
		{
			InGame.PlayerBase.GetBehaviour<PlayerMove>().stop = false;
			thisBase.GetBehaviour<PlayerAnimation>().SetAnmation();
		}
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
			GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderUp, new EventParam() { floatParam = _chargeTime });
		}
	}

	public override void Reset()
	{
		base.Reset();
		_chargeTime = 0;
		thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		_currentVector = Vector3.zero;
		//GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
		InputManager.OnMovePress -= Move;
		InputManager.OnAttackHold -= Charge;
		InputManager.OnAttackRelease -= AttackUP;

		_unitStat.Half = 0;
		_unitStat.onBehaviourEnd = null;
	}
}
