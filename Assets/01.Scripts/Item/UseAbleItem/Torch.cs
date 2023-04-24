using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;

public class Torch : UseAbleItem
{
    private HashSet<Vector3> torchPos = new HashSet<Vector3>();

    public override void UseItem()
    {
        if (InGame.Player.HasState(Actors.Characters.CharacterState.Everything)) return;
        if (torchPos.Contains(InGame.Player.transform.position)) return;
        GameObject torch = Define.GetManager<ResourceManager>().Instantiate("TorchModel");
        torch.transform.position = InGame.Player.transform.position.SetY(0.5f);
        torchPos.Add(InGame.Player.transform.position);
    }
}
