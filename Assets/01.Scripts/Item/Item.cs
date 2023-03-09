using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
	public ItemID ID;
	public string ItemName;
	public float Hp;
	public float Atk;
	public float Afs;
	public float Ats;
	public float Weight;

	public float WeightToSpeed() => Weight switch
	{
		1 => 0.2f,
		2 => 0.25f,
		3 => 0.3f,
		4 => 0.35f,
		5 => 0.45f,
		6 => 0.5f,
		7 => 0.7f,
		8 => 0.8f,
		9 => 0.9f,
		_ => 1f
	};
}
public abstract class Item
{
	public ItemInfo itemInfo;
	public virtual void UseItem()
	{

	}
}
