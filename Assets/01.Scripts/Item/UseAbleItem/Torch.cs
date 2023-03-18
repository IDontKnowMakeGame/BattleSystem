using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;

public class Torch : UseAbleItem
{
    PlayerUseAbleItem playerUseAbleItem = null;

    public Torch(PlayerUseAbleItem _playerUseAbleItem)
    {
        playerUseAbleItem = _playerUseAbleItem;
    }

    public override void UseItem()
    {
        // To Do Pooling.
        Object.Instantiate(playerUseAbleItem.Torch, InGame.Player.transform.position, Quaternion.identity);
    }
}
