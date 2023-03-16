using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquiqmentItem
{
	public ItemInfo WeaponInfo
	{
		get
		{
			return info + _weaponClassLevelInfo + _weaponLevelInfo;
		}
	}
	protected ItemInfo _weaponClassLevelInfo;
	protected ItemInfo _weaponLevelInfo;

	public override void Equiqment()
	{
		LoadWeaponClassLevel();
		LoadWeaponLevel();


	}

	public virtual void LoadWeaponClassLevel()
	{

	}

	public virtual void LoadWeaponLevel()
	{

	}
}
