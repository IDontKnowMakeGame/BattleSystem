using UnityEngine;
using Core;

public class WallObject : InteractionActor
{
    public override void Interact()
    {
        if (!InGame.Player.Position.IsNeighbor(Position)) return;

        this.gameObject.SetActive(false);

        GameObject obj = Define.GetManager<ResourceManager>().Instantiate("WallBrokenObject");
        obj.transform.position = this.transform.position;

		GameObject objs = Define.GetManager<ResourceManager>().Instantiate("BrokenParticle");
		objs.transform.position = this.transform.position;
		base.Interact();
    }
}
