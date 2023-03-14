using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Units.Behaviours.Unit;

public class UnitItem : UnitBehaviour
{
	public Dictionary<ItemID, Item> items = new Dictionary<ItemID, Item>();
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
