using Managements;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void EquipWeapon(string name)
    {
        if (_firstWeapon == name || _secondWeapon == name) return;

        if (_firstWeapon == null)
        {
            _firstWeapon = name;
            _firstWeaponImage.sprite = GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{_firstWeapon}");
        }else if(_secondWeapon == null)
        {
            _secondWeapon = name;
            _secondWeaponImage.sprite = GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{_secondWeapon}");
        }
        SaveWeaponData();
    }

    public void Unmount(string name)
    {
        if(_firstWeapon == name)
        {
            _firstWeapon = null;
            _firstWeaponImage.sprite = null;
        }else if(_secondWeapon == name)
        {
            _secondWeapon = null;
            _secondWeaponImage.sprite = null;
        }
        SaveWeaponData();
    }

    public void SaveWeaponData()
    {
        GameManagement.Instance.GetManager<DataManager>().ChangeUserWeaponData(_firstWeapon);
        GameManagement.Instance.GetManager<DataManager>().ChangeUserWeaponData(_secondWeapon,false);
    }
}
