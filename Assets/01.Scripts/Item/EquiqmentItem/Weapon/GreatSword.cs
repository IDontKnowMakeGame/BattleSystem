using Actors.Characters;
using Core;
using Managements.Managers;
using UnityEngine;
using Acts.Characters.Player;
using System;

public class GreatSword : Weapon
{
	public Vector3 _currrentVector;
	
	public float timer;
	private float _half = 0;
	private int halftime = 10;
	private int addDamage = 0;
	private float addTime = 0;
	private float Damage = 0;

	private SliderObject _sliderObject;

	public override void Init()
	{
		base.Init();
		addDamage = (int)info.Atk / halftime;
		addTime = info.Ats / halftime;
		Damage = info.Atk;
	}

	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("GreatSword");
		switch (KillToLevel(level.killedCount))
		{
			case 1:
				_half = 5;
				break;
			case 2:
				_half = 10;
				break;
			case 3:
				_half = 15;
				break;
			case 4:
				_half = 20;
				break;
			case 5:
				_half = 20;
				_weaponClassLevelInfo.Atk = 20;
				break;
		}
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		if (isEnemy)
			return;
		if (_sliderObject == null)
			_sliderObject = _characterActor.GetComponentInChildren<SliderObject>();
		InputManager<GreatSword>.OnClickPress += AttakStart;
		InputManager<GreatSword>.OnClickHold += Hold;
		InputManager<GreatSword>.OnClickRelease += AttackRealease;
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<GreatSword>.OnClickPress -= AttakStart;
		InputManager<GreatSword>.OnClickHold -= Hold;
		InputManager<GreatSword>.OnClickRelease -= AttackRealease;
	}
	public virtual void AttakStart(Vector3 vec)
	{
		if (_characterActor.HasAnyState())
			return;

		_sliderObject.SliderInit(_stat.ChangeStat.ats);
		_sliderObject.SliderActive(true);
		_characterActor.AddState(CharacterState.Hold);
		_attackInfo.ResetDir();
		_currrentVector = DirReturn(vec);
		ChargeAnimation(_currrentVector);
        _characterActor.GetAct<CharacterStatAct>().Half += _half;
    }
	public virtual void Hold(Vector3 vec)
	{
		//_currrentVector = DirReturn(vec);
		//ChargeAnimation(_currrentVector);
		if (!_characterActor.HasState(CharacterState.Hold))
			return;
		if (timer >= info.Ats)
		{
			_sliderObject.PullSlider(0.1f, true, Color.red);
			return;
		}
		timer += Time.deltaTime;
		_sliderObject.SliderUp(timer);
	}
	public virtual void AttackRealease(Vector3 vec)
	{
		if (!_characterActor.HasState(CharacterState.Hold))
			return;

		if (timer >= addTime)
		{
			_attackInfo.UpStat = new ColliderStat(1, 1, InGame.None, InGame.None);
			_attackInfo.DownStat = new ColliderStat(1, 1, InGame.None, InGame.None);
			_attackInfo.LeftStat = new ColliderStat(1, 1, InGame.None, InGame.None);
			_attackInfo.RightStat = new ColliderStat(1, 1, InGame.None, InGame.None);

			_attackInfo.ResetDir();
			_attackInfo.PressInput = _currrentVector;
			_attackInfo.AddDir(_attackInfo.DirTypes(_currrentVector));

			if(timer >= info.Ats)
			{
				_attackInfo.State = CharacterState.KnockBack;
				_attackInfo.CCInfo = new CCInfo() { knockRange = 1 };
			}
			else
				_attackInfo.CCInfo = new CCInfo() { knockRange = 0 };

			_eventParam.attackParam = _attackInfo;
			info.Atk = addDamage * (int)(timer / addTime);
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.Attack, _eventParam);
			//PlayerAttack.OnAttackEnd += AttackEnd;
		}
		else
        {
			_playerAnimation.Play("Idle");
        }			

		timer = 0;
		_currrentVector = Vector3.zero;
		_characterActor.GetAct<CharacterStatAct>().Half -= _half;
		_characterActor.RemoveState(CharacterState.Hold);

		_sliderObject.PullSlider(0f, false, Color.white);
		_sliderObject.SliderActive(false);
	}

	private void AttackEnd(int id)
	{
		info.Atk = Damage;
		PlayerAttack.OnAttackEnd -= AttackEnd;
	}

	private void ChargeAnimation(Vector3 dir)
    {
		if (dir == Vector3.left)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_playerAnimation.Play("HorizontalCharge");
		}
		else if (dir == Vector3.right)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_playerAnimation.Play("HorizontalCharge");
		}
		else if (dir == Vector3.forward)
		{
			_playerAnimation.Play("UpperCharge");
		}
		else if (dir == Vector3.back)
		{
			_playerAnimation.Play("LowerCharge");
		}
	}
}
