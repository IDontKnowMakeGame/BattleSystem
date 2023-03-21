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
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		if (isEnemy)
			return;
		InputManager<GreatSword>.OnAttackPress += AttakStart;
		InputManager<GreatSword>.OnAttackHold += Hold;
		InputManager<GreatSword>.OnAttackRelease += AttackRealease;
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<GreatSword>.OnAttackPress -= AttakStart;
		InputManager<GreatSword>.OnAttackHold -= Hold;
		InputManager<GreatSword>.OnAttackRelease -= AttackRealease;
	}
	public virtual void AttakStart(Vector3 vec)
	{
		if (_playerActor.HasState(CharacterState.Everything))
			return;		

		_characterActor.AddState(CharacterState.Hold);
		_attackInfo.ResetDir();
		_currrentVector = vec;
		ChargeAnimation(_currrentVector);
	}
	public virtual void Hold(Vector3 vec)
	{
		_characterActor.GetAct<CharacterStatAct>().Half += _half;
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
		_characterActor.GetAct<CharacterStatAct>().Half -= _half;
		_characterActor.RemoveState(CharacterState.Hold);
	}

	private void ChargeAnimation(Vector3 dir)
    {
		if (dir == Vector3.left)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(-1, 1, 1);
			_playerAnimation.Play("VerticalCharge");
		}
		else if (dir == Vector3.right)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(1, 1, 1);
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
