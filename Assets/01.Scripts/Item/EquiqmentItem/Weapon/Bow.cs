using Actors.Characters;
using Actors.Characters.Player;
using Core;
using Managements.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("Bow");
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
				break;
			case 4:
				_weaponClassLevelInfo.Atk = 15;
				break;
			case 5:
				_weaponClassLevelInfo.Atk = 15;
				break;
		}
	}

	public override void LoadWeaponLevel()
	{

	}

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		InputManager<Bow>.OnAttackPress += Shoot;
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		InputManager<TwinSword>.OnAttackPress -= Shoot;
	}

	public virtual void Shoot(Vector3 vec)
	{
		//TODO 이거 애니메이션 끝나고 넣어주는 함수
		Arrow.ShootArrow(vec, _characterActor.Position, _characterActor, info.Afs, info.Atk, 6);
	}
}
