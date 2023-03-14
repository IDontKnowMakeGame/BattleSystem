using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unit.Core.Weapon;
using Units.Behaviours.Unit;

public class UnitItem : UnitBehaviour
{
	public Dictionary<ItemID, Item> items = new Dictionary<ItemID, Item>();
	public Dictionary<ItemID, Weapon> weapons = new Dictionary<ItemID, Weapon>();
	public Dictionary<ItemID, Halo> halo = new Dictionary<ItemID, Halo>();
	public Dictionary<ItemID, UseableItem> useableItem = new Dictionary<ItemID, UseableItem>();
	public override void Awake()
	{
		foreach (ItemID itemName in Enum.GetValues(typeof(ItemID)))
		{
			if (itemName == ItemID.None)
				return;

			Type name = Type.GetType(itemName.ToString());
			Item item = Activator.CreateInstance(name) as Item;
			item.thisBase = ThisBase;
			items.Add(itemName, item);
		}
	}
}
