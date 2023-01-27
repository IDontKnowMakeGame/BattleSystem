using System.Collections;
using System.Collections.Generic;
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
        _equipBtn = transform.Find("WeaponImage").GetComponent<Button>();
        _unmountBtn = transform.Find("WeaponImage").GetComponent<Button>();
    }
    private void Start()
    {
        _weaponImage.sprite = Managements.GameManagement.Instance.GetManager<ResourceManagers>().Load<Sprite>($"{weaponName}");
    }

}
