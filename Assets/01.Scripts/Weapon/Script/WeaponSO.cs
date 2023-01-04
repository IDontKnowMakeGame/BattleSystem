using System;
using System.Collections;
using System.Collections.Generic;
using Unit;
using Unit.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponSO", menuName = "SO/Weapon")]
public class WeaponSO : ScriptableObject
{
	public SwordType type;
	public WeaponBaseStat weaponStat;
}


[Serializable]
public struct WeaponBaseStat
{
    public float damage;
	public float weight;
	public float attackTime;
	public float waitTime;
	public float range;
}