using Actors.Characters;  
using Core;
using Managements.Managers;
using UnityEngine;

public class Bow : Weapon
{
	public bool isShoot = false;

	private bool _isCharge = false;
	protected Vector3 _currentVec;

	private float _currentTimer = 0;

	private EventManager _eventManager => Define.GetManager<EventManager>();
	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("Bow");
		switch (KillToLevel(level.killedCount))
		{
			case 1:
				_weaponClassLevelInfo.Atk = 5;
				break;
			case 2:
				_weaponClassLevelInfo.Atk = 10;
				break;
			case 3:
				_weaponClassLevelInfo.Atk = 10;
				break;
			case 4:
				_weaponClassLevelInfo.Atk = 15;
				break;
			case 5:
				_weaponClassLevelInfo.Atk = 15;
				break;
		}
	}

	public override void LoadWeaponLevel()
	{

	}

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		InputManager<Bow>.OnAttackPress += Shoot;
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		InputManager<Bow>.OnAttackPress -= Shoot;
	}

	public override void Update()
	{
		Charge();
	}

	public virtual void Shoot(Vector3 vec)
	{
		if (_isCharge)
			return;

		_isCharge = true;

		_currentVec = vec;
		_characterActor.AddState(CharacterState.StopMove);
		_characterActor.AddState(CharacterState.Hold);

		_eventManager.TriggerEvent(EventFlag.SliderInit, new EventParam { floatParam = WeaponInfo.Ats });
		_eventManager.TriggerEvent(EventFlag.SliderFalse, new EventParam { boolParam = true });
	}

	private void Charge()
	{
		if (!_isCharge)
			return;

		_currentTimer += Time.deltaTime;
		_eventManager.TriggerEvent(EventFlag.SliderUp, new EventParam { floatParam = _currentTimer });
		if (_currentTimer >= info.Ats)
		{
			_currentTimer = 0;
			_isCharge = false;
			_characterActor.RemoveState(CharacterState.StopMove);
			_characterActor.RemoveState(CharacterState.Hold);
			Arrow.ShootArrow(_currentVec, _characterActor.Position, _characterActor,info.Ats, info.Atk, 6);
			_eventManager.TriggerEvent(EventFlag.SliderFalse, new EventParam { boolParam = false });
		}
	}
}
