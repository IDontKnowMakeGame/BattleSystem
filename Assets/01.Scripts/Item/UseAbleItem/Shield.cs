using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;
using Managements.Managers;

public class Shield : UseAbleItem
{
    private bool use = false;

    public override void UseItem()
    {
        // πÊ«‚
        use = !use;

        if (use)
            InputManager<Weapon>.OnAttackPress += SpawnShield;
        else
            InputManager<Weapon>.OnAttackPress -= SpawnShield;
    }

    private void SpawnShield(Vector3 dir)
    {
        Debug.Log("!!");
        Vector3 spawnPos = (InGame.Player.transform.position - dir).SetY(1);

        GameObject shield = Define.GetManager<ResourceManager>().Instantiate("Shield");
        shield.transform.position = spawnPos;

        use = false;
        InputManager<Weapon>.OnAttackPress -= SpawnShield;
    }
}
