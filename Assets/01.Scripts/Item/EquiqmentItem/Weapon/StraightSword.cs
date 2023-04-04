using Actors.Characters;
using Acts.Characters.Player;
using Core;
using Managements.Managers;
using System;
using System.Collections;
using UnityEngine;

public class StraightSword : Weapon
{
	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("StraightSword");
		switch (KillToLevel(level.killedCount))
		{
			case 1:
				_weaponClassLevelInfo.Atk = 10;
				_weaponClassLevelInfo.Ats -= 0.01f;
				break;
			case 2:
				_weaponClassLevelInfo.Atk = 15;
				_weaponClassLevelInfo.Ats -= 0.03f;
				break;
			case 3:
				_weaponClassLevelInfo.Atk = 20;
				_weaponClassLevelInfo.Ats -= 0.05f;
				break;
			case 4:
				_weaponClassLevelInfo.Atk = 20;
				_weaponClassLevelInfo.Ats -= 0.07f;
				_weaponClassLevelInfo.Afs -= 0.01f;
				break;
			case 5:
				_weaponClassLevelInfo.Atk = 20;
				_weaponClassLevelInfo.Ats -= 0.07f;
				_weaponClassLevelInfo.Afs -= 0.05f;
				break;
		}
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		if (isEnemy)
			return;
		InputManager<StraightSword>.OnAttackPress += Attack;

		PlayerAttack.OnAttackEnd += AttackEnd;
	}

	private void AttackEnd(int obj)
	{
		if (obj != _characterActor.UUID)
			return;

		AtsTimer();
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<StraightSword>.OnAttackPress -= Attack;
		PlayerAttack.OnAttackEnd -= AttackEnd;
	}
	public virtual void Attack(Vector3 vec)
	{
		if (!_attakAble)
			return;

		_attackInfo.UpStat = new ColliderStat(1, 1, InGame.None, InGame.None);
		_attackInfo.DownStat = new ColliderStat(1, 1, InGame.None, InGame.None);
		_attackInfo.LeftStat = new ColliderStat(1, 1, InGame.None, InGame.None);
		_attackInfo.RightStat = new ColliderStat(1, 1, InGame.None, InGame.None);

		_attackInfo.ReachFrame = 5;
		_attackInfo.PressInput = vec;
		_attackInfo.ResetDir();
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		_eventParam.attackParam = _attackInfo;
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.Attack, _eventParam);
	}
}
