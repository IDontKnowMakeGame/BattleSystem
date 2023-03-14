using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemInfo
{
	public string Name;
	public ItemID Id;
	public float Hp;
	public float Atk;
	public float Ats;
	public float Afs;
	public float Weight;

	public float WeightToSpeed
    {
    	get
    	{
    		var speed = (Mathf.Pow(Weight, 2) + 20) * 0.01f;
    		return speed;
    	}
    }
	
	public static ItemInfo operator+(ItemInfo origin, ItemInfo other)
	{
		origin.Hp += other.Hp;
		origin.Atk += other.Atk;
		origin.Ats -= other.Ats;
		origin.Afs -= other.Afs;
		origin.Weight += other.Weight;
		return origin;
	}
}
public abstract class Item
{
	public ItemInfo itemInfo;
	public virtual void UseItem()
	{

	}
}
