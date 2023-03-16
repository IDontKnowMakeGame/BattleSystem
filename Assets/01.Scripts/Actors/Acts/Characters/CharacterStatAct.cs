using Acts.Base;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStatAct : Act
{
	[Header("���⼭�� HP�� �ǵ鿩�ֱ�")]
	[SerializeField]
	private ItemInfo BasicStat;
	public ItemInfo ChangeStat
	{
		get
		{
			return BasicStat + WeaponStat;
		}
	}

	[HideInInspector]
	public ItemInfo WeaponStat = null;
}
