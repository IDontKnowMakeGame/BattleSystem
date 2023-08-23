using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using Actors.Characters;
using Acts.Characters;
using UnityEngine;
using Acts.Characters.Player;
using System.ComponentModel;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class TwinSword : Weapon
{
	private int range = 1;

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
		if (isEnemy)
			return;

		InputManager<TwinSword>.OnMoveHold += AttackVec;
		PlayerMove.OnMoveEnd += OnAttack;

		if (_playerAnimation == null)
			_playerAnimation = _characterActor.GetAct<PlayerAnimation>();
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<TwinSword>.OnMoveHold -= AttackVec;
		PlayerMove.OnMoveEnd -= OnAttack;
	}

	public void AttackVec(Vector3 vec) => _attackInfo.PressInput = vec;

	public void OnAttack(int id, Vector3 vec)
	{
		if (_characterActor == null) return;
		if (_characterActor.HasState(CharacterState.Attack)) return;

		if (id != _characterActor.UUID)
			return;

		_attackInfo.ReachFrame = 1;
		if (_attackInfo.PressInput == Vector3.forward || _attackInfo.PressInput == Vector3.back)
		{
			_attackInfo.ResetDir();
			_attackInfo.LeftStat = new ColliderStat(1, range, 1, 0);
			_attackInfo.RightStat = new ColliderStat(1, range, -1, 0);
			_attackInfo.UpStat = new ColliderStat(1, 1, 0, 1);
			_attackInfo.DownStat = new ColliderStat(1, 1, 0, -1);

			_attackInfo.AddDir(DirType.Left);
			_attackInfo.AddDir(DirType.Right);
		}
		else if (_attackInfo.PressInput == Vector3.left || _attackInfo.PressInput == Vector3.right)
		{
			_attackInfo.ResetDir();
			_attackInfo.UpStat = new ColliderStat(range, 1, 0, 1);
			_attackInfo.DownStat = new ColliderStat(range, 1, 0, -1);
			_attackInfo.LeftStat = new ColliderStat(1, 1, -1, 0);
			_attackInfo.RightStat = new ColliderStat(1, 1, 1, 0);

			_attackInfo.AddDir(DirType.Up);
			_attackInfo.AddDir(DirType.Down);
		}
		_eventParam.attackParam = _attackInfo;
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.FureAttack, _eventParam);

		Define.GetManager<SoundManager>().PlayAtPoint("Sounds/TwinSword/TwinMove", this._characterActor.transform.position, 0.8f);
	}
}
