using UnityEngine;
using Core;
using Actors.Bases;

public class WallObject : Actor,IPickable
{
	public void Mining()
	{
		if (!InGame.Player.Position.IsNeighbor(Position)) return;
		if (!Define.GetManager<DataManager>().HaveUseableItem(Data.ItemID.Pick))
			return;

		this.gameObject.SetActive(false);

		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("WallBrokenObject");
		obj.transform.position = this.transform.position;

		GameObject objs = Define.GetManager<ResourceManager>().Instantiate("BrokenParticle");
		objs.transform.position = this.transform.position;
	}
}
