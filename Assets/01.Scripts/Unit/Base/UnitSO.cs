using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSO", menuName = "SO/UnitSO")]
public class UnitSO : ScriptableObject
{
	[SerializeField]
	private float _baseHp;
	[SerializeField]
	private float _baseDamage;
	public float BaseHP => _baseHp;
	public float BaseDamage => _baseDamage;
}
