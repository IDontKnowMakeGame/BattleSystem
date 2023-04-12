using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.Player;
using Core;

public class Torch : UseAbleItem
{
    public override void UseItem()
    {
        // To Do Pooling.
        GameObject torch = Define.GetManager<ResourceManager>().Instantiate("Torch");
        torch.transform.position = InGame.Player.transform.position;
    }
}
