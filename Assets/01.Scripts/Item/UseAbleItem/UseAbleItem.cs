using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAbleItem : Item
{

	/// <summary>
	/// �⺻������ �������� ����� �� ���� �Լ�
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
