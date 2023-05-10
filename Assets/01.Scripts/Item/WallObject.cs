using UnityEngine;
using Core;
using Actors.Bases;
using Actors.Characters;

public class WallObject : CharacterActor,IPickable
{
	public void Mining()
	{
		if (!InGame.Player.Position.IsNeighbor(Position)) return;
		if (!Define.GetManager<DataManager>().HaveUseableItem(Data.ItemID.Pick))
			return;

		GameObject obj = Define.GetManager<ResourceManager>().Instantiate("WallBrokenObject");
		obj.transform.position = this.transform.position;
		obj.GetComponent<Broken>().Brokens(Vector3.zero, 0);

		this.gameObject.SetActive(false);

	}
}
