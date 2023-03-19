using Actors.Characters;
using Core;
using Managements.Managers;
using UnityEngine;

public class StraightSword : Weapon
{
	public override void Init()
	{
		InputManager<StraightSword>.OnAttackPress += Attack;
	}
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

	public override void LoadWeaponLevel()
	{

	}

	public virtual void Attack(Vector3 vec)
	{
		_attackInfo.SizeX = 1;
		_attackInfo.SizeZ = 1;
		_attackInfo.PressInput = vec;
		_attackInfo.ResetDir();
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		_playerActor.AddState(CharacterState.Attack);
		_eventParam.attackParam = _attackInfo;
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.Attack, _eventParam);
	}
}
