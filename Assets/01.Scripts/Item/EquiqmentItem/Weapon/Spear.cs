using Actors.Characters;
using Blocks;
using Core;
using Managements.Managers;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Spear : Weapon
{
	private bool _isAttack;
	private bool _isEnterEnemy = true;
	private bool _isDown = false;
	private MapManager _mapManager;
	private Vector3 _currentVec = Vector3.zero;

	private Block _attackBlock => _mapManager.GetBlock(_characterActor.Position + _currentVec);
	public override void Init()
	{
		InputManager<Spear>.OnAttackPress += Attack;

		_mapManager = Define.GetManager<MapManager>();
	}

	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("Spear");
		switch (KillToLevel(level.killedCount))
		{
			case 1:
				_weaponClassLevelInfo.Atk = 5;
				_weaponClassLevelInfo.Ats -= 0.01f;
				_weaponClassLevelInfo.Afs -= 0.01f;
				break;
			case 2:
				_weaponClassLevelInfo.Atk = 10;
				_weaponClassLevelInfo.Ats -= 0.03f;
				_weaponClassLevelInfo.Afs -= 0.03f;
				break;
			case 3:
				_weaponClassLevelInfo.Atk = 15;
				_weaponClassLevelInfo.Ats -= 0.05f;
				_weaponClassLevelInfo.Afs -= 0.05f;
				break;
			case 4:
				_weaponClassLevelInfo.Atk = 20;
				_weaponClassLevelInfo.Ats -= 0.07f;
				_weaponClassLevelInfo.Afs -= 0.07f;
				break;
			case 5:
				_weaponClassLevelInfo.Atk = 20;
				_weaponClassLevelInfo.Ats -= 0.07f;
				_weaponClassLevelInfo.Afs -= 0.07f;
				_attackInfo.SizeX = 2;
				_attackInfo.SizeZ = 2;
				break;
		}
	}

	public override void LoadWeaponLevel()
	{

	}

	public override void Update()
	{
		if (_isDown && _attackBlock.IsActorOnBlock && _isEnterEnemy)
		{
			_eventParam.attackParam = _attackInfo;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.Attack, _eventParam);
			_isEnterEnemy = false;
		}
		else if (!_attackBlock.IsActorOnBlock && !_isEnterEnemy)
			_isEnterEnemy = true;
	}

	public virtual void Attack(Vector3 vec)
	{
		if(!_isAttack && !_isDown)
		{
			_isAttack = true;
			_characterActor.StartCoroutine(AttackCorutine(vec));
		}
		else if(_isAttack && _isDown && vec ==_currentVec)
		{
			_isAttack = false;
			_characterActor.StartCoroutine(AttackUpCorutine(vec));
		}
	}

	public virtual IEnumerator AttackCorutine(Vector3 vec)
	{
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		_currentVec = vec;
		yield return new WaitForSeconds(info.Ats);
		_isDown = true;
		Debug.Log("¾î¾ß");
	}

	public virtual IEnumerator AttackUpCorutine(Vector3 vec)
	{
		_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
		_attackInfo.PressInput = vec;
		_currentVec = Vector3.zero;
		yield return new WaitForSeconds(info.Afs);
		_isDown = false;
	}
}
