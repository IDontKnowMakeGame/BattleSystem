using Actors.Characters;
using Core;
using Managements.Managers;
using UnityEngine;
using Actors.Characters.Player;
using Acts.Characters.Player;
using UnityEditor.U2D.Path.GUIFramework;
using Unity.VisualScripting;

public class Bow : Weapon
{
	public bool isShoot = false;
	private bool _isCharge = false;
	private float _currentTimer = 0;

	protected Vector3 _currentVec;
	private Vector3 _orginVec;

	private SliderObject _sliderObject = null;

	public bool isDestroy = false;

	public override void LoadWeaponClassLevel()
	{
		WeaponClassLevelData level = Define.GetManager<DataManager>().LoadWeaponClassLevel("Bow");
		switch (KillToLevel(level.killedCount))
		{
			case 1:
				_weaponClassLevelInfo.Atk = 5;
				break;
			case 2:
				_weaponClassLevelInfo.Atk = 10;
				break;
			case 3:
				_weaponClassLevelInfo.Atk = 10;
				break;
			case 4:
				_weaponClassLevelInfo.Atk = 15;
				break;
			case 5:
				_weaponClassLevelInfo.Atk = 15;
				break;
		}
	}

	public override void Equiqment(CharacterActor actor)
	{
		base.Equiqment(actor);
		if (_sliderObject == null)
		{
		_sliderObject = _characterActor.transform.Find("Anchor").Find("Model").Find("SliderObject").GetComponent<SliderObject>();
		}

		if (isEnemy)
			return;


		InputManager<Bow>.OnAttackPress += Shoot;

		if (_playerAnimation.GetClip("HorizontalPull") == null)
			Debug.Log("»ª!!");

		_unitAnimation.GetClip("HorizontalPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
		_unitAnimation.GetClip("UpperPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
		_unitAnimation.GetClip("LowerPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
		_unitAnimation.GetClip("GroundPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
		_unitAnimation.GetClip("HorizontalPull")?.SetEventOnFrame(_playerAnimation.GetClip("HorizontalPull").fps - 1, SetAnimation);
		_unitAnimation.GetClip("UpperPull")?.SetEventOnFrame(_playerAnimation.GetClip("UpperPull").fps - 1, SetAnimation);
		_unitAnimation.GetClip("LowerPull")?.SetEventOnFrame(_playerAnimation.GetClip("LowerPull").fps - 1, SetAnimation);
		_unitAnimation.GetClip("GroundPull")?.SetEventOnFrame(_playerAnimation.GetClip("GroundPull").fps - 1, SetAnimation);
		SetAnimation();
	}

	private void SetAnimation()
	{
		string str = isShoot ? "None" : "Use";
		_unitAnimation.GetClip("Idle")?.ChangeClip(_unitAnimation.GetClip(str + "Idle"));
		_unitAnimation.GetClip("HorizontalMove")?.ChangeClip(_unitAnimation.GetClip(str + "HorizontalMove"));
		_unitAnimation.GetClip("UpperMove")?.ChangeClip(_unitAnimation.GetClip(str + "UpperMove"));
		_unitAnimation.GetClip("LowerMove")?.ChangeClip(_unitAnimation.GetClip(str + "LowerMove"));
		_unitAnimation.GetClip("HorizontalCharge")?.ChangeClip(_unitAnimation.GetClip(str + "HorizontalCharge"));
		_unitAnimation.GetClip("UpperCharge")?.ChangeClip(_unitAnimation.GetClip(str + "UpperCharge"));
		_unitAnimation.GetClip("LowerCharge")?.ChangeClip(_unitAnimation.GetClip(str + "LowerCharge"));

		_characterActor.RemoveState(CharacterState.StopMove);
		_characterActor.RemoveState(CharacterState.Attack);
	}

	public override void UnEquipment(CharacterActor actor)
	{
		base.UnEquipment(actor);
		if (isEnemy)
			return;
		InputManager<Bow>.OnAttackPress -= Shoot;
	}

	public override void Update()
	{
		base.Update();
		Charge();
	}

	public virtual void Shoot(Vector3 vec)
	{
		if (_isCharge)
			return;

		if (isShoot && _playerActor != null)
			return;

		if (_characterActor.HasState(CharacterState.Everything))
			return;

		_isCharge = true;
		isShoot = true;


		_orginVec = vec;
		_currentVec = InGame.CamDirCheck(vec);
		_characterActor.AddState(CharacterState.StopMove);
		_characterActor.AddState(CharacterState.Hold);

		// Player Animation
		ChargeAnimation(_orginVec);
		//SetAnimation();

		_sliderObject.SliderInit(_stat.ChangeStat.ats);
		_sliderObject.SliderActive(true);
	}

	private void Charge()
	{
		if (!_isCharge)
			return;

		_currentTimer += Time.deltaTime;
		_sliderObject.SliderUp(_currentTimer);
		if (_currentTimer >= info.Ats)
		{
			_currentTimer = 0;
			_isCharge = false;
			_characterActor.RemoveState(CharacterState.StopMove);
			_characterActor.RemoveState(CharacterState.Hold);
			_characterActor.AddState(CharacterState.Attack);
			
			ShootAnimation(_orginVec);
			Arrow.ShootArrow(_currentVec, _characterActor.Position, _characterActor, info.Afs, info.Atk, 6, isDestroy);
			_sliderObject.SliderActive(false);
		}
	}

	private void ChargeAnimation(Vector3 dir)
	{
		if (dir == Vector3.left)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_unitAnimation.Play("HorizontalCharge");
		}
		else if (dir == Vector3.right)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_unitAnimation.Play("HorizontalCharge");
		}
		else if (dir == Vector3.forward)
		{
			_unitAnimation.Play("UpperCharge");
		}
		else if (dir == Vector3.back)
		{
			_unitAnimation.Play("LowerCharge");
		}

	}

	private void ShootAnimation(Vector3 dir)
	{
		if (dir == Vector3.left)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_unitAnimation.Play("HorizontalShoot");
		}
		else if (dir == Vector3.right)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_unitAnimation.Play("HorizontalShoot");
		}
		else if (dir == Vector3.forward)
		{
			_unitAnimation.Play("UpperShoot");
		}
		else if (dir == Vector3.back)
		{
			_unitAnimation.Play("LowerShoot");
		}

		_unitAnimation.curClip.SetEventOnFrame(_unitAnimation.curClip.fps - 1, SetAnimation);
	}
}
