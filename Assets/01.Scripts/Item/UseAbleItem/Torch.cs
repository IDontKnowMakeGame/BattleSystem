using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;
using Managements.Managers;

public class Torch : UseAbleItem
{
    private HashSet<Vector3> torchPos = new HashSet<Vector3>();

    public override bool UseItem()
    {
        if (torchPos.Contains(InGame.Player.Position)) return false;
        var block = InGame.GetBlock(InGame.Player.Position);
        if (block == null) return false;
        if(block.isWalkable == false) return false;
        if(block.isWet) return false;
        GameObject torch = Define.GetManager<ResourceManager>().Instantiate("TorchModel");
        Define.GetManager<SoundManager>().PlayAtPoint("Sounds/item/torch_fire", InGame.Player.Position, true);
        torch.transform.position = InGame.Player.Position.SetY(0.5f);
        torchPos.Add(InGame.Player.Position);
        return true;
    }
}
