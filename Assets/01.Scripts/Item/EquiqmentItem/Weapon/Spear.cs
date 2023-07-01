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
using Tools;

public class Spear : Weapon
{
	private bool _isEnterEnemy = false;
	private bool _isDown = false;
	private bool _isCurrentVec = false;
	private bool _isClick = false;
	private bool _nonDir = false;

	private bool _isAni = false;

	public bool NonDir => _nonDir;

	private MapManager _mapManager => Define.GetManager<MapManager>();
	protected Vector3 _currentVec = Vector3.zero;
	protected Vector3 _originVec = Vector3.zero;

	private int range = 1;
	private string name;

	private PlayerMove _move;

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
		_move = _characterActor?.GetAct<PlayerMove>();

		InputManager<Spear>.OnClickPress += Attack;
		InputManager<Spear>.OnMovePress += CurrentBool;
		CharacterMove.OnMoveEnd += MoveEnd;

		DefaultAnimation();

		Debug.Log("Equiqment");
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;

		InputManager<Spear>.OnClickPress -= Attack;
		InputManager<Spear>.OnMovePress -= CurrentBool;
		CharacterMove.OnMoveEnd -= MoveEnd;
		_isAni = false;

		_currentVec = Vector3.zero;
		_originVec = Vector3.zero;
		_nonDir = false;
		_isDown = false;
		_characterActor.RemoveState(CharacterState.DontMoveAniation);
		Debug.Log("UnEquiqment");
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

		if (_isDown && DirReturn(vec) == _originVec && !_isAni)
		{
			AttackUpCorutine(DirReturn(vec));
		}
		else if (!_isAni && InGame.CamDirCheck(DirReturn(vec)) != _currentVec)
		{
			_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
			_attackInfo.PressInput = vec;
			_currentVec = Vector3.zero;
			_originVec = Vector3.zero;
			_nonDir = false;
			_isDown = false;
			AttackCorutine(DirReturn(vec));
		}

		Debug.Log(!_isAni && InGame.CamDirCheck(DirReturn(vec)) != _currentVec);
		Debug.Log(!_isAni);
		Debug.Log(InGame.CamDirCheck(DirReturn(vec)) != _currentVec);
	}
	public virtual void AttackCorutine(Vector3 vec)
	{
		_attackInfo.AddDir(_attackInfo.DirTypes(vec));
		_originVec = vec;
		_currentVec = InGame.CamDirCheck(_originVec);
		_attackInfo.PressInput = vec;
		_nonDir = true;
		//DefaultAnimation();
		ReadyAnimation(vec);
		_isAni = true;
		Define.GetManager<SoundManager>().PlayAtPoint("Spear/SpearUp", _characterActor.transform.position);
	} 
	public virtual void AttackUpCorutine(Vector3 vec)
	{
		_attackInfo.RemoveDir(_attackInfo.DirTypes(vec));
		_attackInfo.PressInput = vec;
		_currentVec = Vector3.zero;
		_originVec = Vector3.zero;
		_nonDir = false;
		_isDown = false;
		_characterActor.RemoveState(CharacterState.DontMoveAniation);
		DefaultAnimation();
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
		if (_characterActor.HasState(CharacterState.Move))
		{
			Debug.Log(_playerAnimation.curClip.name);
			Debug.Log("Àå¾Ö");
		}

		if (vec == Vector3.left || vec == Vector3.right)
		{
				InGame.Player.SpriteTransform.localScale = vec == Vector3.left ? new Vector3(-2, 1, 1)
	: new Vector3(2, 1, 1);
				_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("HorizontalReadyHorizontalMove"));
				_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("HorizontalReadyUpperMove"));
				_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("HorizontalReadyLowerMove"));
				_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("HorizontalReadyIdle"));
			ClipBase clip = _playerAnimation.GetClip("HorizontalReady");
			clip.SetEventOnFrame(clip.fps - 1, () =>
			{
				_isDown = true;
				_isAni = false;
				_characterActor.RemoveState(CharacterState.DontMoveAniation);
			});
			name = "HorizontalReady";
			_playerAnimation.Play("HorizontalReady");
			_characterActor.AddState(CharacterState.DontMoveAniation);
		}
		else if (vec == Vector3.back)
		{
				_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("LowerReadyHorizontalMove"));
				_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("LowerReadyUpperMove"));
				_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("LowerReadyLowerMove"));
				_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("LowerReadyIdle"));
			ClipBase clip = _playerAnimation.GetClip("LowerReady");
			clip.SetEventOnFrame(clip.fps - 1, () =>
			{
				_isDown = true;
				_isAni = false;
				_characterActor.RemoveState(CharacterState.DontMoveAniation);
			});
			name = "LowerReady";
			_playerAnimation.Play("LowerReady");
			_characterActor.AddState(CharacterState.DontMoveAniation);
		}
		else if (vec == Vector3.forward)
		{
				_playerAnimation.GetClip("HorizontalMove").ChangeClip(_playerAnimation.GetClip("UpperReadyHorizontalMove"));
				_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip("UpperReadyUpperMove"));
				_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip("UpperReadyLowerMove"));
				_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip("UpperReadyIdle"));
			ClipBase clip = _playerAnimation.GetClip("UpperReady");
			clip.SetEventOnFrame(clip.fps - 1, () =>
			{
				_isDown = true;
				_isAni = false;
				_characterActor.RemoveState(CharacterState.DontMoveAniation);
			});
			name = "UpperReady";
			_playerAnimation.Play("UpperReady");
			_characterActor.AddState(CharacterState.DontMoveAniation);
		}

		Debug.Log(_playerAnimation.curClip.name);
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