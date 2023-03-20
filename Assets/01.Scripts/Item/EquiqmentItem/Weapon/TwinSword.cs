using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Actors.Characters;
using Acts.Characters;
using UnityEngine;

public class TwinSword : Weapon
{
	private int range = 1;
	public override void Init()
	{
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.UpArrow);
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.DownArrow);
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.LeftArrow);
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.RightArrow);
	}

	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("TwinSword");
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
				range = 2;
				break;
			case 4:
				_weaponClassLevelInfo.Atk = 15;
				range = 2;
				break;
			case 5:
				_weaponClassLevelInfo.Atk = 15;
				range = 3;
				break;
		}
	}

	public override void LoadWeaponLevel()
	{

	}

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		CharacterMove.OnMoveEnd += Attack;
		Debug.Log("??");
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		CharacterMove.OnMoveEnd -= Attack;
	}

	public virtual void Attack(int id, Vector3 vec)
	{
		if (id != _characterActor.UUID) return;
		if(vec == Vector3.forward || vec == Vector3.back)
		{
			_attackInfo.SizeX = range;
			_attackInfo.ResetDir();
			_attackInfo.AddDir(DirType.Left);
			_attackInfo.AddDir(DirType.Right);
		}
		else if(vec == Vector3.left || vec == Vector3.right)
		{
			_attackInfo.SizeZ = range;
			_attackInfo.ResetDir();
			_attackInfo.AddDir(DirType.Up);
			_attackInfo.AddDir(DirType.Down);
		}

		_attackInfo.PressInput = vec;
		_eventParam.attackParam = _attackInfo;
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.Attack, _eventParam);
	}
}
