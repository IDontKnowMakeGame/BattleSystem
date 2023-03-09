using Core;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using Tool.Data.Json;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : Manager
{
	public Dictionary<ItemID, Weapon> weapons = new Dictionary<ItemID, Weapon>();
	public Dictionary<ItemID, Halo> halos = new Dictionary<ItemID, Halo>();
	public Dictionary<ItemID, UseAbleItem> useAbleItems = new Dictionary<ItemID, UseAbleItem>();
	public override void Awake()
	{
		foreach (ItemID itemID in Enum.GetValues(typeof(ItemID)))
		{
			if(itemID == ItemID.None)
				continue;

			int id = (int)itemID / 100;
			InsertDic(id, itemID);
		}
	}


	private void InsertDic(int id, ItemID itemId)
	{
		switch(id)
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

	private T CreateEnumToClass<T>(ItemID id) where T : Item
	{
		Type type = Type.GetType(id.ToString());
		T instance = Activator.CreateInstance(type) as T;
		instance.itemStat = JsonManager.LoadJsonFile<ItemStat>(Application.dataPath + "/Save", type.ToString());
		instance.ID = id;
		return instance;
	}
}
