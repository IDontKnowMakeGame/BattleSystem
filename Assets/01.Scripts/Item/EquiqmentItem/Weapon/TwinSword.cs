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
		if (_characterActor.HasState(CharacterState.Equip))
			return;

		InputManager<Weapon>.OnMovePress += AttackVec;
		PlayerMove.OnMoveEnd += OnAttack;

		if (_playerAnimation == null)
			_playerAnimation = _characterActor.GetAct<PlayerAnimation>();
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<Weapon>.OnMovePress -= AttackVec;
		PlayerMove.OnMoveEnd -= OnAttack;
	}

	public void AttackVec(Vector3 vec) => _attackInfo.PressInput = vec;

	public void OnAttack(int id, Vector3 vec)
	{
		if (_characterActor == null) return;

		if (id != _characterActor.UUID)
			return;

		int z = (int)_attackInfo.PressInput.z;
		int x = (int)_attackInfo.PressInput.x;
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
	}

	public virtual void Attack(Vector3 vec)
	{
		if (_characterActor.HasState(CharacterState.Move) || _characterActor.HasState(CharacterState.Attack)) return;
		int z = (int)vec.z;
		int x = (int)vec.x;

		_attackInfo.PressInput = vec;
		_attackInfo.ReachFrame = 5;
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			_attackInfo.ResetDir();
			_attackInfo.LeftStat = new ColliderStat(1, range, 1, 0);
			_attackInfo.RightStat = new ColliderStat(1, range, -1, 0);
			_attackInfo.UpStat = new ColliderStat(1, 1, 0, 1);
			_attackInfo.DownStat = new ColliderStat(1, 1, 0, -1);

			_attackInfo.AddDir(DirType.Left);
			_attackInfo.AddDir(DirType.Right);

			_eventParam.attackParam = _attackInfo;
		}
		else if (vec == Vector3.left || vec == Vector3.right)
		{
			_attackInfo.ResetDir();
			_attackInfo.UpStat = new ColliderStat(range, 1, 0, 1);
			_attackInfo.DownStat = new ColliderStat(range, 1, 0, -1);
			_attackInfo.LeftStat = new ColliderStat(1, 1, -1, 0);
			_attackInfo.RightStat = new ColliderStat(1, 1, 1, 0);

			_attackInfo.AddDir(DirType.Up);
			_attackInfo.AddDir(DirType.Down);

			_eventParam.attackParam = _attackInfo;
		}
		_characterActor.StartCoroutine(Attack(_attackInfo.ReachFrame, vec));
	}

	private IEnumerator Attack(int frame, Vector3 vec)
	{
		float time = 0;
		_eventParam.boolParam = false;
		yield return null;
		if (vec == Vector3.forward || vec == Vector3.back)
		{
			float a = _playerAnimation.GetClip(vec == Vector3.forward ? "UpperMove" : "LowerMove").delay;
			time = frame * a;
		}
		else
		{
			float b = _playerAnimation.GetClip("HorizontalMove").delay;
			time = frame * b;
		}
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.NoneAniAttack, _eventParam);
		yield return new WaitForSeconds(time);
		_eventParam.boolParam = true;
		if(_characterActor.HasState(CharacterState.Attack))
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.NoneAniAttack, _eventParam);
		else
			_playerActor.RemoveState(CharacterState.Attack);
	}
}
