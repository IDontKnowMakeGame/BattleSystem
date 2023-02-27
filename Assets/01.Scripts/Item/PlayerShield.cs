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

    public override void Start()
    {
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

    protected override void Use()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            useAble = !useAble;
        }
    }
}
