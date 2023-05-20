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
        if (InGame.Player.HasState(Actors.Characters.CharacterState.Everything)) return false;
        if (torchPos.Contains(InGame.Player.transform.position)) return false;
        GameObject torch = Define.GetManager<ResourceManager>().Instantiate("TorchModel");
        torch.transform.position = InGame.Player.transform.position.SetY(0.5f);
        torchPos.Add(InGame.Player.transform.position);
        return true;
    }
}
