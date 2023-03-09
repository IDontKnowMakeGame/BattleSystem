using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
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
		switch (Define.GetManager<DataManager>().LoadWeaponLevelData(ID))
		{
			case 1:
				itemStat.Atk += 20;
				break;
			case 2:
				itemStat.Atk += 45;
				break;
			case 3:
				itemStat.Atk += 75;
				break;
			case 4:
				itemStat.Atk += 110;
				break;
			case 5:
				itemStat.Atk += 150;
				break;
			case 6:
				itemStat.Atk += 195;
				break;
			case 7:
				itemStat.Atk += 245;
				break;
			case 8:
				itemStat.Atk += 300;
				break;
			case 9:
				itemStat.Atk += 360;
				break;
			case 10:
				itemStat.Atk += 425;
				break;
			case 11:
				itemStat.Atk += 495;
				break;
			case 12:
				itemStat.Atk += 570;
				break;
			default:
				break;
		}
	}
}
