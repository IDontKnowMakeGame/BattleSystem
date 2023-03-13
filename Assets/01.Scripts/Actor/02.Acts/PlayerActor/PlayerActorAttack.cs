using Actor.Acts;
using Actor.Bases;
using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActorAttack : ActorAttack
{
	private PlayerController _playerController;

	private Weapon currentWeapon => _playerController.weapon;
	protected override void Awake()
	{
		base.Awake();
		_playerController = _controller as PlayerController;
		_playerController.OnAttack += SetAttack;
	}
	public void Start()
	{

	}
	public void SetAttack(Vector3 vec, Weapon weapon)
	{
		if (_playerController.HasState(State.Attack))
			return;

		_playerController.AddState(State.Attack);

		if (currentWeapon is BaseStraightSword)
			StraightSword(vec, weapon);
		else if (currentWeapon is BaseGreatSword)
			GreatSword(vec, weapon);
		else if (currentWeapon is BaseTwinSword)
			TwinSword(vec, weapon);
		else if (currentWeapon is BaseSpear)
			Spear(vec, weapon);
		else if (currentWeapon is BaseBow)
			Bow(vec, weapon);
	}

	public void StraightSword(Vector3 vec, Weapon weapon)
	{
		//if(_) TODO 어택 중인지 확인
		StartCoutineAction(null, () => { Attack(vec, weapon.AttackInfo); _playerController.RemoveState(State.Attack); }, weapon.itemInfo.Atk);
	}
	public void GreatSword(Vector3 vec, Weapon weapon)
	{
		//홀드, 키 놨을때
	}
	public void TwinSword(Vector3 vec, Weapon weapon)
	{

	}
	public void Bow(Vector3 vec, Weapon weapon)
	{

	}
	public void Spear(Vector3 vec, Weapon weapon)
	{

	}

	private void StartCoutineAction(Action beforeAction = null, Action afterAction = null, float speed = 1f)
	{
		StartCoroutine(Corutine(beforeAction, afterAction, speed));
	}
	private IEnumerator Corutine(Action beforeAction, Action afterAction, float secound)
	{
		beforeAction?.Invoke();
		yield return new WaitForSeconds(secound);
		afterAction?.Invoke();
	}
}
