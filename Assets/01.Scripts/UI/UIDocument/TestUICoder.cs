using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUICoder : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        //Define.GetManager<DataManager>().AddWeaponToInventory("OldStraightSword");
        //Define.GetManager<DataManager>().AddWeaponToInventory("OldGreatSword");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Define.GetManager<UIManager>().ShowWeaponStore();
        }
    }
}
