using UnityEngine;
using Core;

public class WallObject : InteractionActor
{
    public override void Interact()
    {
        if (!InGame.Player.Position.IsNeighbor(Position)) return;
        GameObject obj = Define.GetManager<ResourceManager>().Instantiate("WallBrokenObject");
        obj.transform.position = this.transform.position;
		GameObject objects = Define.GetManager<ResourceManager>().Instantiate("BrokenParticle");
		objects.transform.position = this.transform.position;
		this.gameObject.SetActive(false);
    }
}
