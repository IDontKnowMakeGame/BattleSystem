using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;
using Managements.Managers;

public class ShieldItem : UseAbleItem
{
    PlayerUseAbleItem playerUseAbleItem = null;

    private bool use = false;

    public ShieldItem(PlayerUseAbleItem _playerUseAbleItem)
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
        // To Do Pooling.
        Vector3 spawnPos = (InGame.Player.transform.position + dir).SetY(1);
        Object.Instantiate(playerUseAbleItem.Shield, spawnPos, Quaternion.identity);

        use = false;
        InputManager<GreatSword>.OnAttackPress -= SpawnShield;
    }
}
