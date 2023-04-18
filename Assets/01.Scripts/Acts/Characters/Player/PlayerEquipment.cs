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
	[SerializeField]
	private HaloRenderer _haloRanderer;

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

	private EventParam _eventParam = new EventParam();

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

		_useHalo.Add(ItemID.HaloOfGhost, new HaloOfGhost());
		_useHalo.Add(ItemID.HaloOfPollution, new HaloOfPollution());
		_useHalo.Add(ItemID.HaloOfEreshkigal, new HaloOfEreshkigal());
	}
	private int count = 0;
	public override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.P))
		{
			switch (count++)
			{
				case 0:
					AddHalo(ItemID.HaloOfPollution);
					break;
				case 1:
					AddHalo(ItemID.HaloOfGhost);
					break;
				case 2:
					AddHalo(ItemID.HaloOfEreshkigal);
					break;
				default:
					count = 0;
					break;
			}
		}
	}
	public override void OnDisable()
	{
		Define.GetManager<EventManager>()?.StopListening(EventFlag.WeaponEquip, EquipmentWeapon);
		base.OnDisable();
	}
	#endregion
	#region Weapon
	public void WeaponOnOff()
	{
		Debug.Log("?");
		if (_haveinHand)
		{
			Debug.Log("UnEquipment");
			CurrentWeapon?.UnEquipment(_characterController);
			Debug.Log(CurrentWeapon);
			_characterController.currentWeapon = null;
			_haveinHand = !_haveinHand;
		}
		else
		{
			_haveinHand = !_haveinHand;
			CurrentWeapon?.Equiqment(_characterController);
		}
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.ChangeStat, _eventParam);
	}
	public override void Change()
	{
		if (!_haveinHand)
			return;
		if (_playerActor.HasAnyState()) return;
		base.Change();
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.ChangeStat, _eventParam);
		Define.GetManager<DataManager>().SwapWeaponData();
		UIManager.Instance.UpdateInGameUI();
	}
	private void Skill()
	{
		if (!_haveinHand)
			return;
		CurrentWeapon?.Skill();
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.ChangeStat, _eventParam);
	}
	private void Upgrade(EventParam eventParam)
	{
		CurrentWeapon?.LoadWeaponLevel();
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.ChangeStat, _eventParam);
	}
	#endregion
	#region WeaponEquipment
	protected virtual void EquipmentWeapon(EventParam eventParam)
	{
		//TODO ���⼭ EventParam�� �޾��ִµ� �׶� ���⼭ �������� ������ �ε����� ���� ������ �־��ش�.
		if (_firstWeapon == ItemID.None)
		{
			_firstWeapon = DataManager.UserData_.firstWeapon;
			CurrentWeapon.Equiqment(_characterController);
		}

		if (_secondWeapon == ItemID.None)
		{
			_secondWeapon = DataManager.UserData_.firstWeapon;
		}

		CurrentWeapon.UnEquipment(_characterController);
		_firstWeapon = DataManager.UserData_.firstWeapon;
		_secondWeapon = DataManager.UserData_.secondWeapon;
		EquipAnimation();
		CurrentWeapon.Equiqment(_characterController);

		Define.GetManager<EventManager>().TriggerEvent(EventFlag.ChangeStat, _eventParam);
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

	#region Halo
	public void AddHalo(ItemID haloID)
    {
		_halos.Add(haloID);
		_haloRanderer?.EquipmentHalo(haloID);
		_useHalo[haloID].Equiqment(_characterController);
    }

	public void RemoveHalo(ItemID haloID)
    {
		if(_halos.Contains(haloID))
        {
			_useHalo[haloID].UnEquipment(_characterController);
			_halos.Remove(haloID);
        }

    }
	#endregion
	#region HaloEquipment
	protected override void EquipmentHalo()
	{
		base.EquipmentHalo();
		Define.GetManager<EventManager>().TriggerEvent(EventFlag.ChangeStat, _eventParam);
	}
	#endregion
}
