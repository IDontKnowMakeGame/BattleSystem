using Actors.Characters;
using Core;
using Managements.Managers;
using UnityEngine;
using Acts.Characters.Player;

public class GreatSword : Weapon
{
	public Vector3 _currrentVector;
	public float timer;
	private float _half = 0;
	public override void Init()
	{
		InputManager<GreatSword>.OnAttackPress += AttakStart;
		InputManager<GreatSword>.OnAttackHold += Hold;
		InputManager<GreatSword>.OnAttackRelease += AttackRealease;
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

	public override void LoadWeaponLevel()
	{

	}

	public virtual void AttakStart(Vector3 vec)
	{
		if (_playerActor.HasState(CharacterState.Hold))
			return;

		_playerActor.AddState(CharacterState.Hold);
		_attackInfo.ResetDir();
		_currrentVector = vec;
		ChargeAnimation(_currrentVector);
	}
	public virtual void Hold(Vector3 vec)
	{
		_playerActor.GetAct<CharacterStatAct>().Half += _half;
		if (timer >= info.Ats)
			return;
		timer += Time.deltaTime;
	}
	public virtual void AttackRealease(Vector3 vec)
	{
		if(timer >= info.Ats)
		{
			_attackInfo.SizeX = 1;
			_attackInfo.SizeZ = 1;
			_attackInfo.ResetDir();
			_attackInfo.PressInput = vec;
			_attackInfo.AddDir(_attackInfo.DirTypes(_currrentVector));

			_eventParam.attackParam = _attackInfo;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.Attack, _eventParam);
		}
		else
        {
			_playerAnimation.Play("Idle");
        }			

		timer = 0;
		_currrentVector = Vector3.zero;
		_playerActor.GetAct<CharacterStatAct>().Half -= _half;
		_playerActor.RemoveState(CharacterState.Hold);
	}

	private void ChargeAnimation(Vector3 dir)
    {
		if (dir == Vector3.left)
		{
			_playerActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
			_playerAnimation.Play("VerticalCharge");
		}
		else if (dir == Vector3.right)
		{
			_playerActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
			_playerAnimation.Play("VerticalCharge");
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
