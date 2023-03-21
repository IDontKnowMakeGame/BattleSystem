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

[Serializable]
public class PlayerEquipment : CharacterEquipmentAct
{
	protected PlayerActor _playerActor;
	private PlayerAnimation _playerAnimation;
	public override void Start()
	{
		base.Start();
		_playerActor = _characterController as PlayerActor;
		_playerAnimation = ThisActor.GetAct<PlayerAnimation>();
		InputManager<Weapon>.OnChangePress += Change;
		Define.GetManager<EventManager>().StartListening(EventFlag.WeaponEquip, EquipmentWeapon);
		EquipAnimation();
	}

	public override void Update()
	{
		CurrentWeapon?.Update();
	}

	public override void Change()
	{
		if (_playerActor.HasAnyState()) return;
		base.Change();
		EquipAnimation();
	}

	#region Equipment
	protected virtual void EquipmentWeapon(EventParam eventParam)
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if (_firstWeapon == ItemID.None /*&& evnetParam.intparam == 1*/)
		{
			//firstWeapon = Datamanger.Instnace.firstWeapon;
			CurrentWeapon.Equiqment(_characterController);
		}

		if (_secondWeapon == ItemID.None /*&& evnetParam.intparam == 2*/)
		{
			//secoundWeapon = Datamanger.Instnace.firstWeapon;
		}

		CurrentWeapon.UnEquipment(_characterController);
		//firstWeapon = DataManager.Instance.firstWeaopn;
		//secondWeapon = DataManager.Instance.secoundWeaopn;
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
	#endregion

	private void Skill()
	{
		CurrentWeapon?.Skill();
	}
	private void EquipAnimation()
	{
		_playerActor.AddState(CharacterState.Equip);

		_playerAnimation.ChangeWeaponClips((int)_firstWeapon);
		_playerAnimation.Play("Equip");

		// ������ �����ӿ� ���� �ֱ�
		_playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1, () => _playerActor.RemoveState(CharacterState.Equip));
	}
	public override void OnDisable()
	{
		Define.GetManager<EventManager>().StopListening(EventFlag.WeaponEquip, EquipmentWeapon);
		base.OnDisable();
	}
}
