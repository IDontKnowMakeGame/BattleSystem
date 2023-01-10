using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class TestCode : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WeaponStateData data =  GameManagement.Instance.GetManager<DataManager>().GetWeaponStateData("sword");
            Debug.Log($"{data.name} : {data.damage} : {data.attackSpeed} : {data.attackAfterDelay} : {data.weaponClass} : {data.weaponWeight}" );
        }
    }
}
