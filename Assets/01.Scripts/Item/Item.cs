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

	public float WeightToSpeed
    {
    	get
    	{
    		var speed = (Mathf.Pow(Weight, 2) + 20) * 0.01f;
    		return speed;
    	}
    }
}
public abstract class Item
{
	public ItemInfo itemInfo;
	public virtual void UseItem()
	{

	}
}
