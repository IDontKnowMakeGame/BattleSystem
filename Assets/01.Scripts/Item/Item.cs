using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// item�� �⺻���� ������ �������ִ� Ŭ����
/// </summary>
public class Item
{
	public ItemInfo info;
	public Item(ItemInfo info)
	{
		this.info = info;
	}

	/// <summary>
	/// Start���� �����ٴ��� awake���� �����ٰų�
	/// </summary>
	protected virtual void Init()
	{

	}
}
