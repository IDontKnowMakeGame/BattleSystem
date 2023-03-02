using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;
using Units.Base.Unit;

[System.Serializable]
public class PlayerShield : UsableItem
{
    private bool useAble = false;
    public bool UseAble => useAble;

    private GameObject shield;

    ItemInfo itemInfo;

    public override void Start()
    {
        //itemInfo = DataManager.UserData.equipUseableItem[2];

        InputManager.OnAttackPress += CheckShield;
    }

    private void CheckShield(Vector3 dir)
    {
        if(useAble)
        {
            shield = Core.Define.GetManager<ResourceManagers>().Instantiate("Shield");
            Shield shieldUnit = shield.GetComponent<Shield>();
            Vector3 pos = (InGame.PlayerBase.Position + dir).SetY(1);
            shieldUnit.SpawnPos = pos;
            //shield.transform.position = pos;
            InGame.SetUnit(shieldUnit, pos);
            useAble = false;
        }
    }

    public override void Use()
    {
        useAble = !useAble;
    }
}
