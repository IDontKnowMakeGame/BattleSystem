using UnityEngine;
using Core;

public class WallObject : InteractionActor
{
    public override void Interact()
    {
        if (!InGame.Player.Position.IsNeighbor(Position)) return;
        this.gameObject.SetActive(false);
    }
}
