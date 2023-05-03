using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbleItem : Item
{

	/// <summary>
	/// 기본적으로 아이템을 사용할 때 쓰는 함수
	/// </summary>
	public virtual bool UseItem()
	{
		return false;
	}

	public virtual void SettingItem()
    {

    }

	public virtual void UpdateItem()
    {

    }
}
