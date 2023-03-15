using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbleItem : Item
{
	public UseAbleItem(ItemInfo info) : base(info)
	{

	}

	/// <summary>
	/// 기본적으로 아이템을 사용할 때 쓰는 함수
	/// </summary>
	protected virtual void UseItem()
	{

	}
}
