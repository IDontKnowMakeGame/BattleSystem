using Actors.Characters;
using Blocks;
using Core;
using Managements.Managers;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Acts.Characters.Player;
using Acts.Characters;

public class Spear : Weapon
{
	private bool _isAttack;
	private bool _isEnterEnemy = true;
	private bool _isDown = false;
	private bool _isCurrentVec = false;
	private bool _isClick = false;
	private bool _nonDir = false;

	public bool NonDir => _nonDir;

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
		InputManager<Spear>.OnMovePress += CurrentBool;
		CharacterMove.OnMoveEnd += MoveEnd;

		DefaultAnimation();
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;

		InputManager<Spear>.OnAttackPress -= Attack;
		InputManager<Spear>.OnMovePress -= CurrentBool;
		CharacterMove.OnMoveEnd -= MoveEnd;
		_isCurrentVec = false;
		_isAttack = false;
        _isDown = false;
		_nonDir = false;
	}
    public override void Update()
	{
		if (!_isDown)
			return;

		bool isEnemy = false;
		for (int i = 1; i <= range; i++)
		{
			if (_mapManager == null) return;
			if (_mapManager.GetBlock(_characterActor.Position + _currentVec * i)?.ActorOnBlock != null)
				isEnemy = true;
		}
		if (_isDown && _isEnterEnemy && isEnemy && _isCurrentVec)
		{
			_eventParam.attackParam = _attackInfo;
			_isEnterEnemy = false;
			Define.GetManager<EventManager>().TriggerEvent(EventFlag.FureAttack, _eventParam);
		}
		else if (!_isEnterEnemy && !isEnemy)
			_isEnterEnemy = true;
	}

	public virtual void Attack(Vector3 vec)
	{
		if (_characterActor.HasState(CharacterState.Equip))
			return;

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
		_attackInfo.PressInput = vec;
		_nonDir = true;
		ReadyAnimation(vec);
		yield return new WaitForSeconds(info.Ats);
		_isDown = true;
	}

	private void DefaultAnimation()
	{
		_playerAnimation.ChangeWeaponClips((int)info.Id);
		_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("DefaultHorizontalMove"));
		_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("DefaultUpperMove"));
		_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("DefaultLowerMove"));
		_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("DefaultIdle"));
	}

	private void ReadyAnimation(Vector3 vec)
	{
		if (vec == Vector3.left || vec == Vector3.right)
		{
			InGame.Player.SpriteTransform.localScale = vec == Vector3.left ? new Vector3(-2, 1, 1)
				: new Vector3(2, 1, 1);
			_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("HorizontalReadyHorizontalMove"));
			_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("HorizontalReadyUpperMove"));
			_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("HorizontalReadyLowerMove"));
			_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("HorizontalReadyIdle"));
			_playerAnimation.Play("HorizontalReady");
		}
		else if (vec == Vector3.back)
		{
			_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("LowerReadyHorizontalMove"));
			_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("LowerReadyUpperMove"));
			_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("LowerReadyLowerMove"));
			_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("LowerReadyIdle"));
			_playerAnimation.Play("LowerReady");
		}
		else if (vec == Vector3.forward)
		{
			_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("UpperReadyHorizontalMove"));
			_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("UpperReadyUpperMove"));
			_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("UpperReadyLowerMove"));
			_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("UpperReadyIdle"));
			_playerAnimation.Play("UpperReady");
		}
	}

	public virtual IEnumerator AttackUpCorutine(Vector3 vec)
	{
		_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
		_attackInfo.PressInput = vec;
		_currentVec = Vector3.zero;
		_nonDir = false;
		DefaultAnimation();
		yield return new WaitForSeconds(info.Afs);
		_isDown = false;
	}

	private void CurrentBool(Vector3 vec)
	{
		_isClick = true;

		if (!(vec == _attackInfo.PressInput && _isDown))
			_isCurrentVec = false;
		else
			_isCurrentVec = true;
	}

	private void MoveEnd(int id, Vector3 vec)
	{
		if (_characterActor == null)
			return;
		if (id != _characterActor.UUID)
			return;

		_isClick = false;
	}
}
