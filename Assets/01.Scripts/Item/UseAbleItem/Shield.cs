using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;
using Managements.Managers;

public class Shield : UseAbleItem
{
    PlayerUseAbleItem playerUseAbleItem = null;

    private bool use = false;

    public Shield(PlayerUseAbleItem _playerUseAbleItem)
    {
        playerUseAbleItem = _playerUseAbleItem;
    }

    public override void UseItem()
    {
        // πÊ«‚
        use = !use;

        if (use)
            InputManager<GreatSword>.OnAttackPress += SpawnShield;
        else
            InputManager<GreatSword>.OnAttackPress -= SpawnShield;
    }

    private void SpawnShield(Vector3 dir)
    {
        Vector3 spawnPos = (InGame.Player.transform.position + dir).SetY(1);

        GameObject shield = Define.GetManager<ResourceManager>().Instantiate("Shield");
        shield.transform.position = spawnPos;

        use = false;
        InputManager<GreatSword>.OnAttackPress -= SpawnShield;
    }
}
