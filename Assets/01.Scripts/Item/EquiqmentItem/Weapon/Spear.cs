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

	public bool IsDown => _isDown;

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

				_attackInfo.UpStat = new ColliderStat(2, 2, InGame.None, InGame.None);
				_attackInfo.DownStat = new ColliderStat(2, 2, InGame.None, InGame.None);
				_attackInfo.LeftStat = new ColliderStat(2, 2, InGame.None, InGame.None);
				_attackInfo.RightStat = new ColliderStat(2, 2, InGame.None, InGame.None);
				break;
		}
	}
	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		if (isEnemy)
			return;

		_playerAnimation = _characterActor?.GetAct<PlayerAnimation>();

		InputManager<Spear>.OnAttackPress += Attack;

		DefaultAnimation();
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<Spear>.OnAttackPress -= Attack;
		_isAttack = false;
		_isDown = false;
	}
	public override void Update()
	{
		if (_isDown)
			Debug.DrawLine(_characterActor.transform.position, _characterActor.transform.position + _currentVec * range);

		bool isEnemy = false;
		for(int i =1; i<=range; i++)
		{
			if (_mapManager.GetBlock(_characterActor.Position + _currentVec * i)?.ActorOnBlock)
				isEnemy = true;
		}

		if (_isDown && _isEnterEnemy && isEnemy)
		{
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

	private void DefaultAnimation()
    {
		_playerAnimation.GetClip("VerticalMove").ChangeClip(_playerAnimation.GetClip("DefaultVerticalMove"));
		_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("DefaultUpperMove"));
		_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("DefaultLowerMove"));
		_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("DefaultIdle"));
	}

	private void ReadyAnimation(Vector3 vec)
    {
		if (vec == Vector3.left || vec == Vector3.right)
		{
			InGame.Player.SpriteTransform.localScale = vec == Vector3.left ? new Vector3(2, 1, 1) 
				: new Vector3(-2, 1, 1);
			_playerAnimation.GetClip("VerticalMove").ChangeClip(_playerAnimation.GetClip("VerticalReadyVerticalMove"));
			_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("VerticalReadyUpperMove"));
			_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("VerticalReadyLowerMove"));
			_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("VerticalReadyIdle"));
			_playerAnimation.Play("VerticalReady");
		}
		else if (vec == Vector3.back)
		{
			_playerAnimation.GetClip("VerticalMove").ChangeClip(_playerAnimation.GetClip("UpperReadyVerticalMove"));
			_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("UpperReadyUpperMove"));
			_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("UpperReadyLowerMove"));
			_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("UpperReadyIdle"));
			_playerAnimation.Play("UpperReady");
		}
		else if(vec == Vector3.forward)
        {
			_playerAnimation.GetClip("VerticalMove").ChangeClip(_playerAnimation.GetClip("LowerReadyVerticalMove"));
			_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("LowerReadyUpperMove"));
			_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("LowerReadyLowerMove"));
			_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("LowerReadyIdle"));
			_playerAnimation.Play("LowerReady");
		}
	}

	public virtual IEnumerator AttackUpCorutine(Vector3 vec)
	{
		_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
		_attackInfo.PressInput = vec;
		_currentVec = Vector3.zero;
		DefaultAnimation();
		yield return new WaitForSeconds(info.Afs);
		_isDown = false;
	}
}
