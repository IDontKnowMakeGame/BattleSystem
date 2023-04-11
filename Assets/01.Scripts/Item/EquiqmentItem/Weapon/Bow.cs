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
		if (isEnemy)
			return;

		Debug.Log("?");
		if (_sliderObject == null)
		{
			Debug.Log(_characterActor.transform.Find("Model"));
			Debug.Log(_characterActor.transform.Find("Model").Find("SliderObject"));
			Debug.Log(_characterActor.transform.Find("Model").Find("SliderObject").GetComponent<SliderObject>());
			_sliderObject = _characterActor.transform.Find("Model").Find("SliderObject").GetComponent<SliderObject>();
		}

		InputManager<Bow>.OnAttackPress += Shoot;

		if (actor is PlayerActor)
		{
			_playerAnimation.GetClip("VerticalPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
			_playerAnimation.GetClip("UpperPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
			_playerAnimation.GetClip("LowerPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
			_playerAnimation.GetClip("GroundPull")?.SetEventOnFrame(0, () => _characterActor.AddState(CharacterState.StopMove));
			_playerAnimation.GetClip("VerticalPull")?.SetEventOnFrame(_playerAnimation.GetClip("VerticalPull").fps - 1, SetAnimation);
			_playerAnimation.GetClip("UpperPull")?.SetEventOnFrame(_playerAnimation.GetClip("UpperPull").fps - 1, SetAnimation);
			_playerAnimation.GetClip("LowerPull")?.SetEventOnFrame(_playerAnimation.GetClip("LowerPull").fps - 1, SetAnimation);
			_playerAnimation.GetClip("GroundPull")?.SetEventOnFrame(_playerAnimation.GetClip("GroundPull").fps - 1, SetAnimation);
			SetAnimation();
		}
	}

	private void SetAnimation()
	{
		string str = isShoot ? "None" : "Use";

		_playerAnimation.GetClip("Idle").ChangeClip(_playerAnimation.GetClip(str + "Idle"));
		_playerAnimation.GetClip("VerticalMove").ChangeClip(_playerAnimation.GetClip(str + "VerticalMove"));
		_playerAnimation.GetClip("UpperMove").ChangeClip(_playerAnimation.GetClip(str + "UpperMove"));
		_playerAnimation.GetClip("LowerMove").ChangeClip(_playerAnimation.GetClip(str + "LowerMove"));
		_playerAnimation.GetClip("VerticalCharge").ChangeClip(_playerAnimation.GetClip(str + "VerticalCharge"));
		_playerAnimation.GetClip("UpperCharge").ChangeClip(_playerAnimation.GetClip(str + "UpperCharge"));
		_playerAnimation.GetClip("LowerCharge").ChangeClip(_playerAnimation.GetClip(str + "LowerCharge"));

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

		if (isShoot)
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
		if (_characterActor is PlayerActor)
		{
			ChargeAnimation(_orginVec);
			//SetAnimation();
		}

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
			Arrow.ShootArrow(_currentVec, _characterActor.Position, _characterActor, info.Afs, info.Atk, 6);
			_sliderObject.SliderActive(false);
		}
	}

	private void ChargeAnimation(Vector3 dir)
	{
		if (dir == Vector3.left)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_playerAnimation.Play("VerticalCharge");
		}
		else if (dir == Vector3.right)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_playerAnimation.Play("VerticalCharge");
		}
		else if (dir == Vector3.forward)
		{
			_playerAnimation.Play("UpperCharge");
		}
		else if (dir == Vector3.back)
		{
			_playerAnimation.Play("LowerCharge");
		}

	}

	private void ShootAnimation(Vector3 dir)
	{
		if (dir == Vector3.left)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(-2, 1, 1);
			_playerAnimation.Play("VerticalShoot");
		}
		else if (dir == Vector3.right)
		{
			_characterActor.SpriteTransform.localScale = new Vector3(2, 1, 1);
			_playerAnimation.Play("VerticalShoot");
		}
		else if (dir == Vector3.forward)
		{
			_playerAnimation.Play("UpperShoot");
		}
		else if (dir == Vector3.back)
		{
			_playerAnimation.Play("LowerShoot");
		}

		_playerAnimation.CurrentClip().SetEventOnFrame(_playerAnimation.CurrentClip().fps - 1, SetAnimation);
	}
}
