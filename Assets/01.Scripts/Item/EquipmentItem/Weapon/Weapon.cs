using Actor.Bases;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
	protected WeaponClassLevel _weaponClassLevel;

	protected AttackInfo _attackInfo = new AttackInfo();

	public AttackInfo AttackInfo => _attackInfo;
	
	protected ActorController _actController = null;

	public virtual void Init(ActorController actContorller)
	{
		_actController = actContorller;
	}

	public override void UseItem()
	{
		if (_actController is PlayerController)
			(_actController as PlayerController).OnSkill += Skill;
	}
	public virtual void Skill()
	{

	}

	protected virtual void ClassLevelSystem()
	{

	}

	protected int CountToLevel(int count) => count switch
	{
		<= 40 => 1,
		<= 50 => 2,
		<= 60 => 3,
		<= 70 => 4,
		<= 80 => 5,
		_ => 1
	};

	protected void WeaponLevelSystem()
	{
		switch (Define.GetManager<DataManager>().LoadWeaponLevelData(itemInfo.Id))
		{
			case 1:
				itemInfo.Atk += 20;
				break;
			case 2:
				itemInfo.Atk += 45;
				break;
			case 3:
				itemInfo.Atk += 75;
				break;
			case 4:
				itemInfo.Atk += 110;
				break;
			case 5:
				itemInfo.Atk += 150;
				break;
			case 6:
				itemInfo.Atk += 195;
				break;
			case 7:
				itemInfo.Atk += 245;
				break;
			case 8:
				itemInfo.Atk += 300;
				break;
			case 9:
				itemInfo.Atk += 360;
				break;
			case 10:
				itemInfo.Atk += 425;
				break;
			case 11:
				itemInfo.Atk += 495;
				break;
			case 12:
				itemInfo.Atk += 570;
				break;
			default:
				break;
		}
	}
}
