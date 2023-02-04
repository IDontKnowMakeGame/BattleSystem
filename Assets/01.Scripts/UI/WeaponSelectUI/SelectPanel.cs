using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    private string weaponName;

    Image _weaponImage;
    Button _equipBtn;
    Button _unmountBtn;

    private void Awake()
    {
        weaponName = gameObject.name;
        _weaponImage = transform.Find("WeaponImage").GetComponent<Image>();
        _equipBtn = transform.Find("Equip").GetComponent<Button>();
        _unmountBtn = transform.Find("Unmount").GetComponent<Button>();

        _equipBtn.onClick.AddListener(Equip);
        _unmountBtn.onClick.AddListener(Unmount);
    }
    private void Start()
    {
        _weaponImage.sprite = Managements.GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{weaponName}");
    }

    private void Equip()
    {
        EventParam param = new EventParam();
        param.stringParam = weaponName;
        Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponEquip, param);
    }

    private void Unmount()
    {
        EventParam param = new EventParam();
        param.stringParam = weaponName;
        Define.GetManager<EventManager>().TriggerEvent(EventFlag.WeaponUnmount, param);
    }
}
