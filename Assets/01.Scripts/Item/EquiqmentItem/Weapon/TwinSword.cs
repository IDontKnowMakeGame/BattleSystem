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

	public virtual void Attack(Vector3 vec)
	{
		if (_characterActor.HasState(CharacterState.Everything)) return;
		Vector3 vector = InGame.CamDirCheck(vec);
		int z = (int)vector.z;
		int x = (int)vector.x;

		_attackInfo.PressInput = vec;
		_attackInfo.ReachFrame = 4;
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			_attackInfo.ResetDir();
			if (range == 3)
			{
				if (vec == Vector3.forward)
				{
					_attackInfo.AddDir(DirType.Up);
				}
				else
				{
					_attackInfo.AddDir(DirType.Down);
				}
				_attackInfo.LeftStat = new ColliderStat(1, 2, z, x);
				_attackInfo.RightStat = new ColliderStat(1, 2, -z, -x);
				_attackInfo.UpStat = new ColliderStat(1, 1, 0, 1);
				_attackInfo.DownStat = new ColliderStat(1, 1, 0, -1);
			}
			else
			{
				_attackInfo.LeftStat = new ColliderStat(InGame.None, range, InGame.None, InGame.None);
				_attackInfo.RightStat = new ColliderStat(InGame.None, range, InGame.None, InGame.None);
			}

			_attackInfo.AddDir(DirType.Left);
			_attackInfo.AddDir(DirType.Right);

			_eventParam.attackParam = _attackInfo;
		}
		else if (vec == Vector3.left || vec == Vector3.right)
		{
			_attackInfo.ResetDir();
			if (range == 3)
			{
				_attackInfo.AddDir(_attackInfo.DirTypes(vec));
				_attackInfo.UpStat = new ColliderStat(2, 1, z, x);
				_attackInfo.DownStat = new ColliderStat(2, 1, -z, -x);
				_attackInfo.LeftStat = new ColliderStat(1, 1, 1, 0);
				_attackInfo.RightStat = new ColliderStat(1, 1, -1, 0);
			}
			else
			{
				_attackInfo.UpStat = new ColliderStat(InGame.None, range, InGame.None, InGame.None);
				_attackInfo.DownStat = new ColliderStat(InGame.None, range, InGame.None, InGame.None);

				_attackInfo.AddDir(DirType.Up);
				_attackInfo.AddDir(DirType.Down);

				_eventParam.attackParam = _attackInfo;
			}
		}
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.NoneAniAttack, _eventParam);
	}
}
