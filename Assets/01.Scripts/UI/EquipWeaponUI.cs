using Managements;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class EquipWeaponUI : MonoBehaviour
{


    [SerializeField]
    private Image _firstWeaponImage;
    private string _firstWeapon;

    [SerializeField]
    private Image _secondWeaponImage;
    private string _secondWeapon;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        User data = DataManager.UserData;
        _firstWeapon = data.firstWeapon;
        _secondWeapon = data.secondWeapon;

        Debug.Log($"{data.firstWeapon} + {data.secondWeapon}");
        _firstWeaponImage.sprite = GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{_firstWeapon}");
        _secondWeaponImage.sprite = GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{_secondWeapon}");

        Define.GetManager<EventManager>().StartListening(EventFlag.WeaponEquip, Equip);
        Define.GetManager<EventManager>().StartListening(EventFlag.WeaponUnmount, Unmount);
        Define.GetManager<EventManager>().StartListening(EventFlag.WeaponSwap, Swap);
    }

    public void Equip(EventParam param)
    {
        string name = param.stringParam;
        if (_firstWeapon == name || _secondWeapon == name) return;

        if ( _firstWeapon == null || _firstWeapon == "" )
        {
            
            _firstWeapon = name;
            _firstWeaponImage.sprite = GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{_firstWeapon}");

            param.intParam = 1;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponChange, param);
        }else if(_secondWeapon == null || _secondWeapon == "")
        {
            _secondWeapon = name;
            _secondWeaponImage.sprite = GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{_secondWeapon}");

            param.intParam = 2;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponChange, param);
        }
        SaveWeaponData();
    }
    public void Unmount(EventParam param)
    {
        string name = param.stringParam;
        if (_firstWeapon == name)
        {
            _firstWeapon = null;
            _firstWeaponImage.sprite = null;

            param.intParam = 1;
            param.stringParam = null;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponChange, param);
        }
        else if(_secondWeapon == name)
        {
            _secondWeapon = null;
            _secondWeaponImage.sprite = null;

            param.intParam = 2;
            param.stringParam = null;
            Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponChange, param);
        }
        SaveWeaponData();
    }
    public void Swap(EventParam param)
    {
        Sprite sprite = _firstWeaponImage.sprite;
        _firstWeaponImage.sprite = _secondWeaponImage.sprite;
        _secondWeaponImage.sprite = sprite;

        SaveWeaponData();
    }

    public void SaveWeaponData()
    {
        GameManagement.Instance.GetManager<DataManager>().ChangeUserWeaponData(_firstWeapon);
        GameManagement.Instance.GetManager<DataManager>().ChangeUserWeaponData(_secondWeapon,false);
    }
}
