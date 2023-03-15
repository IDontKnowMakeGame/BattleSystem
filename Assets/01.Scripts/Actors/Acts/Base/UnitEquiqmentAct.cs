using Actors.Bases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;

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
	/// ������������� Equiqment�� �� ������� ���� ���ſ� �ٲܰŸ� ���ش�.
	/// </summary>
	protected void EquiqmentItem()
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if(firstWeapon == ItemId.None /*&& evnetParam.intparam == 1*/)
		{

		}

		if(secondWeapon == ItemId.None /*&& evnetParam.intparam == 2*/)
		{

		}
	}
}
