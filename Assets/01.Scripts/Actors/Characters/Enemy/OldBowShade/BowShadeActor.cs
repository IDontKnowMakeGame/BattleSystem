using Actors.Characters;
using Actors.Characters.Enemy;
using Acts.Characters;
using AI.States;
using Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BowShadeActor : EnemyActor
{
	[SerializeField]
	private float _dmage;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private int _range;
	private UnitAnimation _unitAnimation;
	private CharacterStatAct _unitStat;
	private bool _isCharge = false;
	private float _currentTimer = 0;

	private SliderObject _sliderObject = null;

	private Vector3 originVec;

	protected override void Init()
	{
		base.Init();
		AddAct(_enemyAi);
	}

	protected override void Start()
	{
		base.Start();
		ShootState state = _enemyAi.GetState<ShootState>();
		WaitState idlestate = _enemyAi.GetState<WaitState>();
		_unitAnimation = GetAct<UnitAnimation>();
		_unitStat = GetAct<CharacterStatAct>();
		_characterEquipment.CurrentWeapon.Equiqment(this);
		state.OnEnter += () =>
		{
			Shoot(state);
		};
		idlestate.OnEnter += () =>
		{
			//_enemyAnimation.Play("Idle");
		};

		if (_sliderObject == null)
		{
			_sliderObject = transform.Find("Anchor").Find("Model").Find("SliderObject").GetComponent<SliderObject>();
		}
	}

	private void Shoot(ShootState state)
	{
		originVec = InGame.Player.transform.position- this.transform.position;
		Vector3 dir = originVec.normalized;
		dir = InGame.CamDirCheck(dir);
		if (dir.x != 0 && dir.z != 0)
		{
			state?.OnExit?.Invoke();
			return;
		}

		if (dir == Vector3.left || dir == Vector3.right)
		{
			Debug.Log("charge");
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

		dir.y = 0;

		Vector3 cameraDir = InGame.CameraDir();

		var degree = Mathf.Atan2(cameraDir.x, cameraDir.z) * Mathf.Rad2Deg;
		degree = Mathf.Abs(Mathf.RoundToInt(degree));

		if (degree == 90)
		{
			dir.x = -dir.x;
			dir.z = -dir.z;
		}  

		Vector3 vector = this.transform.localScale;
		Vector3 vec = dir.x < 0 ? new Vector3(vector.x, vector.y, vector.z) : new Vector3(Mathf.Abs(vector.x) * -1, vector.y, vector.z);
		this.transform.localScale = vec;


		_sliderObject.SliderInit(_unitStat.ChangeStat.ats);
		_sliderObject.SliderActive(true);

		_isCharge = true;
	}

	protected override void Update()
	{
		base.Update();
		if (!_isCharge)
			return;

		_currentTimer += Time.deltaTime;
		_sliderObject.SliderUp(_currentTimer);
		if (_currentTimer >= _characterEquipment.CurrentWeapon.info.Ats)
		{
			_currentTimer = 0;
			_isCharge = false;

			Arrow.ShootArrow(originVec.normalized,Position, this, _speed, _characterEquipment.CurrentWeapon.info.Atk, _range, true);
			ShootAnimation(InGame.CamDirCheck(originVec.normalized));
			_sliderObject.SliderActive(false);
		}
	}

	private void ShootAnimation(Vector3 dir)
	{
		if (dir == Vector3.left)
		{
			_unitAnimation.Play("HorizontalShoot");
		}
		else if (dir == Vector3.right)
		{
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

		_unitAnimation.GetClip("HorizontalShoot").SetEventOnFrame(_unitAnimation.GetClip("HorizontalShoot").fps - 1, () => _unitAnimation.Play("Idle"));
		_unitAnimation.GetClip("UpperShoot").SetEventOnFrame(_unitAnimation.GetClip("UpperShoot").fps - 1, () => _unitAnimation.Play("Idle"));
		_unitAnimation.GetClip("LowerShoot").SetEventOnFrame(_unitAnimation.GetClip("LowerShoot").fps - 1, () => _unitAnimation.Play("Idle"));
	}
}
