using Actors.Characters;
using Core;
using Managements.Managers;
using UnityEngine;
using Acts.Characters.Player;
using Unity.VisualScripting;

public class Spear : Weapon
{
	private bool _nonDir = false;

	public bool NonDir => _nonDir;

	private MapManager _mapManager => Define.GetManager<MapManager>();
	private Vector3 _currentVec = Vector3.zero;
	private Vector3 _originVec = Vector3.zero;

	private int range = 1;

	public float timer;
	private SliderObject _sliderObject;
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

		InputManager<Spear>.OnClickPress += Attack;

		DefaultAnimation();
	}
	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;

		InputManager<Spear>.OnClickPress -= Attack;
	}
    public override void Update()
	{
	}

	protected void AttackStart(Vector3 vec)
	{
		_characterActor.AddState(CharacterState.Hold);
	}
	protected void AttackRelase(Vector3 vec)
	{
		if (!_characterActor.HasState(CharacterState.Hold))
			return;
		if (timer >= info.Ats)
		{
			_sliderObject.PullSlider(0.1f, true, Color.red);
			return;
		}
		timer += Time.deltaTime;
		_sliderObject.SliderUp(timer);
	}
	public virtual void Attack(Vector3 vec)
	{
		_characterActor.RemoveState(CharacterState.Hold);
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
}
