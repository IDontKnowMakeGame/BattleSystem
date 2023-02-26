using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managements.Managers;
using Core;

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
            shield.transform.position = (InGame.PlayerBase.Position + dir).SetY(1);
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
