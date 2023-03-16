using Acts.Base;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStat
{
	public float hp;
	public float atk;
	public float ats;
	public float afs;
	public float speed;
}

[Serializable]
public class CharacterStatAct : Act
{
	[Header("���⼭�� HP�� �ǵ鿩�ֱ�")]
	[SerializeField]
	private CharacterStat BasicStat;

	public CharacterStat ChangeStat;
}
