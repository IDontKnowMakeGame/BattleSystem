using Actors.Bases;
using System.Collections;
using System.Collections.Generic;
using Acts.Base;
using UnityEngine;

public class UnitEquiqmentAct : Act
{
	public Weapon CurrentWeapon
	{
		get
		{
			if (firstWeapon == ItemId.None)
				return null;

			Weapon weapon;
			_useWeapon.TryGetValue(firstWeapon, out weapon);
			if(weapon == null)
			{
				weapon = ItemManager.manager.weapons[firstWeapon];
				_useWeapon.Add(firstWeapon, ItemManager.manager.weapons[firstWeapon]);
			}

			return weapon;
		}
	}

	public Weapon SecoundWeapon
	{
		get
		{
			if (secondWeapon == ItemId.None)
				return null;

			Weapon weapon;
			_useWeapon.TryGetValue(secondWeapon, out weapon);
			if (weapon == null)
			{
				weapon = ItemManager.manager.weapons[secondWeapon];
				_useWeapon.Add(secondWeapon, ItemManager.manager.weapons[secondWeapon]);
			}

			return weapon;
		}
	}

	protected Dictionary<ItemId,Weapon> _useWeapon = new Dictionary<ItemId,Weapon>();

	protected ItemId firstWeapon;
	protected ItemId secondWeapon;

	public void Change()
	{
		CurrentWeapon?.UnEquipment();
		SecoundWeapon?.Equiqment();

		ItemId weapon = firstWeapon;
		firstWeapon = secondWeapon;
		secondWeapon = weapon;
	}

	/// <summary>
	/// 비어져있을때는 Equiqment를 안 비어있을 때는 뺄거와 바꿀거를 해준다.
	/// </summary>
	protected void EquiqmentItem()
	{
		//TODO 여기서 EventParam을 받아주는데 그때 여기서 변경해줄 무기의 인덱스와 무기 종류를 넣어준다.
		if(firstWeapon == ItemId.None /*&& evnetParam.intparam == 1*/)
		{

		}

		if(secondWeapon == ItemId.None /*&& evnetParam.intparam == 2*/)
		{

		}
	}
}
