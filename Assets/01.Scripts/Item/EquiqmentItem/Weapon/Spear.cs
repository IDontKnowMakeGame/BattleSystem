using Actors.Characters;
using Blocks;
using Core;
using Managements.Managers;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Acts.Characters.Player;

public class Spear : Weapon
{
	private bool _isAttack;
	private bool _isEnterEnemy = true;
	private bool _isDown = false;
	private MapManager _mapManager => Define.GetManager<MapManager>();
	private Vector3 _currentVec = Vector3.zero;

	private int range = 1;
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
				range = 2;
				break;
		}
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);

		_playerAnimation = _characterActor.GetAct<PlayerAnimation>();

		InputManager<Spear>.OnAttackPress += Attack;

		_playerAnimation.GetClip("VerticalMove").texture = _playerAnimation.GetClip("DefaultVerticalMove").texture;
		_playerAnimation.GetClip("UpperMove").texture = _playerAnimation.GetClip("DefaultUpperMove").texture;
		_playerAnimation.GetClip("LowerMove").texture = _playerAnimation.GetClip("DefaultLowerMove").texture;
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		InputManager<Spear>.OnAttackPress -= Attack;
	}
	public override void Update()
	{
		if (_isDown)
			Debug.DrawLine(_characterActor.transform.position, _characterActor.transform.position + _currentVec * range);

		bool isEnemy = false;
		for(int i =1; i<=range; i++)
		{
			if (_mapManager.GetBlock(_characterActor.Position + _currentVec * i).ActorOnBlock)
				isEnemy = true;
		}

		if (_isDown && _isEnterEnemy && isEnemy)
		{
			if (range == 2 && _currentVec == Vector3.forward || _currentVec == Vector3.back)
			{
				_attackInfo.SizeZ = range;
				_attackInfo.SizeX = 1;
			}
			else if(range == 2)
			{
				_attackInfo.SizeX = range;
				_attackInfo.SizeZ = 1;
			}
			_eventParam.attackParam = _attackInfo;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.FureAttack, _eventParam);
			_isEnterEnemy = false;
		}
		else if (!_isEnterEnemy && !isEnemy)
			_isEnterEnemy = true;
	}

	public virtual void Attack(Vector3 vec)
	{
		if (!_isAttack && !_isDown)
		{
			_isAttack = true;
			_characterActor.StartCoroutine(AttackCorutine(vec));
		}
		else if (_isAttack && _isDown && InGame.CamDirCheck(vec) == _currentVec)
		{
			_isAttack = false;
			_characterActor.StartCoroutine(AttackUpCorutine(vec));
		}
	}

	public virtual IEnumerator AttackCorutine(Vector3 vec)
	{
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		_currentVec = InGame.CamDirCheck(vec);
		ReadyAnimation(_currentVec);
		yield return new WaitForSeconds(info.Ats);
		_isDown = true;
	}

	private void ReadyAnimation(Vector3 vec)
    {
		if (vec == Vector3.left || vec == Vector3.right)
		{
			_playerAnimation.GetClip("VerticalMove").texture = _playerAnimation.GetClip("VerticalReadyVerticalMove").texture;
			_playerAnimation.GetClip("UpperMove").texture = _playerAnimation.GetClip("VerticalReadyUpperMove").texture;
			_playerAnimation.GetClip("LowerMove").texture = _playerAnimation.GetClip("VerticalReadyLowerMove").texture;
		}
		else if (vec == Vector3.forward)
		{
			_playerAnimation.GetClip("VerticalMove").texture = _playerAnimation.GetClip("UpperReadyVerticalMove").texture;
			_playerAnimation.GetClip("UpperMove").texture = _playerAnimation.GetClip("UpperReadyUpperMove").texture;
			_playerAnimation.GetClip("LowerMove").texture = _playerAnimation.GetClip("UpperReadyLowerMove").texture;
		}
		else if(vec == Vector3.back)
        {
			_playerAnimation.GetClip("VerticalMove").texture = _playerAnimation.GetClip("LowerReadyVerticalMove").texture;
			_playerAnimation.GetClip("UpperMove").texture = _playerAnimation.GetClip("LowerReadyUpperMove").texture;
			_playerAnimation.GetClip("LowerMove").texture = _playerAnimation.GetClip("LowerReadyLowerMove").texture;
		}
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
