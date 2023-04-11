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
				Weapon weapon = CreateEnumToClass<Weapon>(itemId, id);
				weapon.Init();
				weapons.Add(itemId, weapon);
				break;
			case 1:
				Halo halo = CreateEnumToClass<Halo>(itemId, id);
				Debug.Log(halo);
				halo.Init();
				halos.Add(itemId, halo);
				break;
			case 2:
				UseAbleItem useable = CreateEnumToClass<UseAbleItem>(itemId, id);
				useable.Init();
				useAbleItems.Add(itemId, useable);
				break;
			default:
				break;
		}
	}

	private T CreateEnumToClass<T>(ItemID id, int index) where T : Item, new()
	{
		Type name = Type.GetType(id.ToString());
		T instance = Activator.CreateInstance(name) as T;
		if(index == 0)
        {
			ItemTable table = JsonManager.LoadJsonFile<ItemTable>(Application.streamingAssetsPath + "/Save/Json/" + typeof(ItemTable), typeof(ItemTable).ToString());
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
		return instance;
	}
}
