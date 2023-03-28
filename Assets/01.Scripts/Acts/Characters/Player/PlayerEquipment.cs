using Actors.Characters.Player;
using Actors.Characters;
using Acts.Characters.Player;
using Core;
using Data;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.TextCore.Text;

[Serializable]
public class PlayerEquipment : CharacterEquipmentAct
{
	protected PlayerActor _playerActor;
	private PlayerAnimation _playerAnimation;

	private bool _haveinHand = true;

	public override Weapon CurrentWeapon
	{
		get
		{
			if (!_haveinHand)
				return null;
			return base.CurrentWeapon;
		}
	}

	#region Life Cycle
	public override void Start()
	{
		_firstWeapon = DataManager.UserData_.firstWeapon;
		_secondWeapon = DataManager.UserData_.secondWeapon;
		_playerActor = _characterController as PlayerActor;
		_playerAnimation = ThisActor.GetAct<PlayerAnimation>();
		EquipAnimation();
		base.Start();
		InputManager<Weapon>.OnChangePress += Change;
		InputManager<Weapon>.OnSkillPress += Skill;
		InputManager<Weapon>.OnOffPress += WeaponOnOff;
		Define.GetManager<EventManager>().StartListening(EventFlag.WeaponEquip, EquipmentWeapon);
		Define.GetManager<EventManager>().StartListening(EventFlag.WeaponUpgrade, Upgrade);
	}
	public override void Update()
	{
		CurrentWeapon?.Update();
	}
	public override void OnDisable()
	{
		Define.GetManager<EventManager>()?.StopListening(EventFlag.WeaponEquip, EquipmentWeapon);
		base.OnDisable();
	}
	#endregion

	#region Equipment
	protected virtual void EquipmentWeapon(EventParam eventParam)
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if (_firstWeapon == ItemID.None )
		{
            _firstWeapon = DataManager.UserData_.firstWeapon;
            CurrentWeapon.Equiqment(_characterController);
		}

		if (_secondWeapon == ItemID.None )
		{
			_secondWeapon = DataManager.UserData_.firstWeapon;
		}

		CurrentWeapon.UnEquipment(_characterController);
		_firstWeapon = DataManager.UserData_.firstWeapon;
		_secondWeapon = DataManager.UserData_.secondWeapon;
        CurrentWeapon.Equiqment(_characterController);
	}

	/// <summary>
	/// ������������� Equiqment�� �� ������� ���� ���ſ� �ٲܰŸ� ���ش�.
	/// </summary>
	protected virtual void EquipmentWeapon()
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if (_firstWeapon == ItemID.None)
		{
			//firstWeapon = Datamanger.Instnace.firstWeapon;
		}

		if (_secondWeapon == ItemID.None)
		{
			//secoundWeapon = Datamanger.Instnace.firstWeapon;
		}

		CurrentWeapon?.UnEquipment(_characterController);
		//firstWeapon = DataManager.Instance.firstWeaopn;
		//secondWeapon = DataManager.Instance.secoundWeaopn;
		CurrentWeapon?.Equiqment(_characterController);
	}
	protected override void EquipAnimation()
	{
		_playerActor.AddState(CharacterState.Equip);

		_playerAnimation.ChangeWeaponClips((int)_firstWeapon);
		_playerAnimation.Play("Equip");

		// ������ �����ӿ� ���� �ֱ�
		_playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1, () => _playerActor.RemoveState(CharacterState.Equip));
	}
	#endregion

	public void WeaponOnOff()
	{
		if (_haveinHand)
		{
			CurrentWeapon?.UnEquipment(_characterController);
			_characterController.currentWeapon = null;
		}
		else
		{
			CurrentWeapon?.Equiqment(_characterController);
		}
		_haveinHand = !_haveinHand;
	}
	public override void Change()
	{
		if (!_haveinHand)
			return;
		if (_playerActor.HasAnyState()) return;
		base.Change();
	}
	private void Skill()
	{
		if (!_haveinHand)
			return;
		CurrentWeapon?.Skill();
	}
	private void Upgrade(EventParam eventParam)
	{
		CurrentWeapon?.LoadWeaponLevel();
	}
}
