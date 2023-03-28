using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Actors.Characters;
using Acts.Characters;
using UnityEngine;
using Acts.Characters.Player;

public class TwinSword : Weapon
{
	private int range = 1;
	private PlayerAnimation _playerAnimation;

	public override void Init()
	{
		base.Init(); 
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackForward, KeyCode.UpArrow);
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackBackward, KeyCode.DownArrow);
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackLeft, KeyCode.LeftArrow);
		InputManager<TwinSword>.ChangeKeyCode(KeyboardInput.AttackRight, KeyCode.RightArrow);
	}

	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("TwinSword");
		Debug.Log(KillToLevel(level.killedCount));
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

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		InputManager<TwinSword>.OnMovePress += Attack;
		if (_playerAnimation == null)
			_playerAnimation = _characterActor.GetAct<PlayerAnimation>();
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		InputManager<TwinSword>.OnMovePress -= Attack;
	}

	public virtual void Attack(/*int id,*/ Vector3 vec)
	{
		if (_characterActor.HasState(CharacterState.Attack)) return;
		if(vec == Vector3.forward || vec == Vector3.back)
		{
			_attackInfo.LeftStat = new ColliderStat(range, InGame.None, InGame.None, InGame.None);
			_attackInfo.RightStat = new ColliderStat(range, InGame.None, InGame.None, InGame.None);
			_attackInfo.ResetDir();

			_attackInfo.AddDir(DirType.Left);
			_attackInfo.AddDir(DirType.Right);

			_playerAnimation.GetClip(vec == Vector3.forward ? "UpperMove" : "LowerMove").SetEventOnFrame(2, () => Define.GetManager<EventManager>().TriggerEvent(EventFlag.NoneAniAttack, _eventParam));
		}
		else if(vec == Vector3.left || vec == Vector3.right)
		{
			_attackInfo.UpStat = new ColliderStat(InGame.None, range, InGame.None, InGame.None);
			_attackInfo.DownStat = new ColliderStat(InGame.None, range, InGame.None, InGame.None);
			_attackInfo.ResetDir();


			_attackInfo.AddDir(DirType.Up);
			_attackInfo.AddDir(DirType.Down);

			_playerAnimation.GetClip("VerticalMove").SetEventOnFrame(2, () => Define.GetManager<EventManager>().TriggerEvent(EventFlag.NoneAniAttack, _eventParam));
		}

		_attackInfo.PressInput = vec;
		_attackInfo.ReachFrame = 0;
		_eventParam.attackParam = _attackInfo;
	}
}
