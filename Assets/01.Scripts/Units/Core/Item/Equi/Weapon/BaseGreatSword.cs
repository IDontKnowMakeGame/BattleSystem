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
		_inputManager.ChangeInGameKey(InputTarget.UpAttack, KeyCode.W);
		_inputManager.ChangeInGameKey(InputTarget.DownAttack, KeyCode.S);
		_inputManager.ChangeInGameKey(InputTarget.LeftAttack, KeyCode.A);
		_inputManager.ChangeInGameKey(InputTarget.RightAttack, KeyCode.D);

		_inputManager.AddInGameAction(InputTarget.UpMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.DownMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.LeftMove, InputStatus.Press, Move);
		_inputManager.AddInGameAction(InputTarget.RightMove, InputStatus.Press, Move);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Hold, Charge);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Hold, Charge);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Hold, Charge);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Hold, Charge);

		_inputManager.AddInGameAction(InputTarget.UpAttack, InputStatus.Release, AttackUP);
		_inputManager.AddInGameAction(InputTarget.DownAttack, InputStatus.Release, AttackUP);
		_inputManager.AddInGameAction(InputTarget.LeftAttack, InputStatus.Release, AttackUP);
		_inputManager.AddInGameAction(InputTarget.RightAttack, InputStatus.Release, AttackUP);

		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderInit, new EventParam() { floatParam = _maxChargeTime });
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
	}
	private void Move()
	{
		if (!_thisBase.State.HasFlag(Units.Base.Unit.BaseState.Skill))
		{
			_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
			_chargeTime = 0;
		}
	}

	protected override void UpAttack()
	{
		Attack(Vector3.forward);
	}

	protected override void DownAttack()
	{
		Attack(Vector3.back);
	}

	protected override void LeftAttack()
	{
		Attack(Vector3.left);
	}

	protected override void RightAttack()
	{
		Attack(Vector3.right);
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

	private void AttackUP()
	{
		_thisBase.RemoveState(Units.Base.Unit.BaseState.Charge);
		if(_chargeTime >= _maxChargeTime)
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
		_chargeTime = 0;
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderUp, new EventParam() { floatParam = _chargeTime });
		GameManagement.Instance.GetManager<EventManager>().TriggerEvent(EventFlag.SliderFalse, new EventParam() { boolParam = false });
	}

	private void Charge()
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

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Hold, UpAttack);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Hold, DownAttack);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Hold, LeftAttack);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Hold, RightAttack);

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Release, AttackUP);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Release, AttackUP);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Release, AttackUP);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Release, AttackUP);

		_inputManager.RemoveInGameAction(InputTarget.UpAttack, InputStatus.Press, Move);
		_inputManager.RemoveInGameAction(InputTarget.DownAttack, InputStatus.Press, Move);
		_inputManager.RemoveInGameAction(InputTarget.LeftAttack, InputStatus.Press, Move);
		_inputManager.RemoveInGameAction(InputTarget.RightAttack, InputStatus.Press, Move);
	}
}
