using Actors.Characters;
using Core;
using Managements.Managers;
using UnityEngine;
using Actors.Characters.Player;
using Acts.Characters.Player;
using Unity.VisualScripting;
using Acts.Characters;

public class Bow : Weapon
{
	public bool isShoot = false;
	private bool _isCharge = false;
	private float _currentTimer = 0;


	private bool isScale = true;

	public bool IsScale
	{
		get
		{
			return isScale;
		}
		set
		{
			isScale = value;
		}
	}

	protected Vector3 _currentVec;
	private Vector3 _orginVec;

	private SliderObject _sliderObject = null;

	public bool isDestroy = false;

	public float Damage
	{
		get
		{
			if (isEnemy) return _damage;
			else return info.Atk;
		}
		set
		{
			_damage = value;
		}
	}
	public float Speed {
		
		get 
		{ 
			if (isEnemy) return _speed;
			else return info.Afs;
		} 
		set 
		{ 
			_speed = value; 
		} 
	}

	public int Range { 
		get 
		{
			if (isEnemy)
				return _range;
			else
				return 6;
		} 
		set 
		{
			_range = value;
		} 
	}

	private float _damage = 0;
	private float _speed = 0;
	private int _range = 0;

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


		InputManager<Bow>.OnClickPress += Shoot;

		if (_playerAnimation.GetClip("HorizontalPull") == null)
			Debug.Log("빽!!");

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
		InputManager<Bow>.OnClickPress -= Shoot;

		_currentTimer = 0;
		_isCharge = false;
		_characterActor.RemoveState(CharacterState.StopMove);
		_characterActor.RemoveState(CharacterState.Hold);
		_sliderObject.SliderActive(false);
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


		_orginVec = DirReturn(vec);
		_currentVec = InGame.CamDirCheck(_orginVec);
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
			Arrow.ShootArrow(_currentVec, _characterActor.Position, _characterActor, Speed, Damage, Range, isDestroy);
			_sliderObject.SliderActive(false);
		}
	}

	private void ChargeAnimation(Vector3 dir)
	{
		//Debug.Log(InGame.CamDirCheck(dir));

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
		Vector3 cameraDir = InGame.CameraDir();

		var degree = Mathf.Atan2(cameraDir.x, cameraDir.z) * Mathf.Rad2Deg;
		degree = Mathf.Abs(Mathf.RoundToInt(degree));

		if(degree == 90)
		{
			dir.x = -dir.x;
			dir.z = -dir.z;
		}
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
