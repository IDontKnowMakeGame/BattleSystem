using Data;
using Managements.Managers;
using System;
using System.Collections.Generic;
using Tool.Data.Json.ParsingList;
using Tool.Data.Json;
using UnityEngine;

public class ItemManager : Manager
{
	public Dictionary<ItemID, Item> items = new Dictionary<ItemID, Item>();
	public Dictionary<ItemID, Weapon> weapons = new Dictionary<ItemID, Weapon>();
	public Dictionary<ItemID, Halo> halos = new Dictionary<ItemID, Halo>();
	public Dictionary<ItemID, UseAbleItem> useAbleItems = new Dictionary<ItemID, UseAbleItem>();

	public override void Awake()
	{
		base.Awake();
		foreach (ItemID itemID in Enum.GetValues(typeof(ItemID)))
		{
			if (itemID == ItemID.None)
				continue;

			int id = (int)itemID / 100;
			InsertDic(id, itemID);
		}
	}

	public Weapon GetWeapon(ItemID id)
	{
		Weapon weapon = null;
		weapons.TryGetValue(id, out weapon);
		return weapon;
	}


	private void InsertDic(int id, ItemID itemId)
	{
		switch (id)
		{
			case 0:
				weapons.Add(itemId, CreateEnumToClass<Weapon>(itemId));
				break;
			case 1:
				halos.Add(itemId, CreateEnumToClass<Halo>(itemId));
				break;
			case 2:
				useAbleItems.Add(itemId, CreateEnumToClass<UseAbleItem>(itemId));
				break;
			default:
				break;
		}
	}

	private T CreateEnumToClass<T>(ItemID id) where T : Item, new()
	{
		Type name = Type.GetType(id.ToString());
		T instance = Activator.CreateInstance(name) as T;
		ItemTable table = JsonManager.LoadJsonFile<ItemTable>(Application.dataPath + "/Save/Json/" + typeof(ItemTable), typeof(ItemTable).ToString());
		foreach (var item in table.ItemList)
		{
			if (item.Id == id)
			{
				instance.info = item;
				items.Add(id, instance);
				return instance;
			}
		}
		return null;
	}
}
