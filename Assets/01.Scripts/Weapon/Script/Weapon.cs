using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Manager;
using Unit;

[System.Serializable]
public class Weapon
{
	public Unit.Unit _baseObject;
	protected InputManager _inputManager;

	protected UnitMove _move;
	protected UnitAttack _attack;

	protected float _currentTime;
	protected float _maxTime;

	protected bool _isCoolTime = false;

	public bool isSkill = false;

	public bool _isEnemy = false;

	protected WeaponStateData _basicData;
	public Weapon(Unit.Unit unit)
	{
		SetBase(unit);
	}

	public Weapon()
	{

	}

	public virtual void Awake()
	{

	}

	public virtual void Start()
	{
		_inputManager = GameManagement.Instance.GetManager<InputManager>();
		_move = _baseObject.GetBehaviour<UnitMove>();
		_attack = _baseObject.GetBehaviour<UnitAttack>();
	}

	public virtual void Update()
	{

	}


	protected virtual void Move()
	{

	}

	protected virtual void Attack()
	{

	}

	protected virtual void Skill()
	{

	}

	protected void Timer()
	{
		if (_currentTime < _maxTime && _isCoolTime)
		{
			_currentTime += Time.deltaTime;
		}
		else
		{
			_isCoolTime = false;
			_currentTime = 0;
		}
	}

	public void SetBase(Unit.Unit _base)
	{
		_baseObject = _base;
	}

	public void GetWeaponStateData(string str)
	{
		_baseObject.StartCoroutine(Coroutine(str));
	}

	IEnumerator Coroutine(string str)
	{
		yield return new WaitUntil(()=>GameManagement.Instance.GetManager<DataManager>().isSettingComplate);

		_basicData = GameManagement.Instance.GetManager<DataManager>().GetWeaponStateData(str);
		Debug.Log($"{_basicData.name} : {_basicData.damage} : {_basicData.attackSpeed} : {_basicData.attackAfterDelay} : {_basicData.weaponClass}: {_basicData.weaponWeight}");
	}
}
