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

		AddHalo(ItemID.HaloOfPollution);
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
		//TODO 여기서 EventParam을 받아주는데 그때 여기서 변경해줄 무기의 인덱스와 무기 종류를 넣어준다.
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
	/// 비어져있을때는 Equiqment를 안 비어있을 때는 뺄거와 바꿀거를 해준다.
	/// </summary>
	protected virtual void EquipmentWeapon()
	{
		//TODO 여기서 EventParam을 받아주는데 그때 여기서 변경해줄 무기의 인덱스와 무기 종류를 넣어준다.
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

		// 마지막 프레임에 종료 넣기
		_playerAnimation.curClip.SetEventOnFrame(_playerAnimation.curClip.fps - 1, () => _playerActor.RemoveState(CharacterState.Equip));
	}
	#endregion

	#region Halo
	public void AddHalo(ItemID haloID)
    {
		_halos.Add(haloID);
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
