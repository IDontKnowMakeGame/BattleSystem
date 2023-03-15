using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class ItemManager : MonoBehaviour
{
	public Dictionary<ItemId, Item> items = new Dictionary<ItemId, Item>();
	public Dictionary<ItemId, Weapon> weapons = new Dictionary<ItemId, Weapon>();
	public Dictionary<ItemId, Halo> halos = new Dictionary<ItemId, Halo>();
	public Dictionary<ItemId, UseAbleItem> useableItems = new Dictionary<ItemId, UseAbleItem>();

	public static ItemManager manager;

	private void Awake()
	{
		manager = GetComponent<ItemManager>();
	}
}
