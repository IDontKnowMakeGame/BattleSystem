using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[System.Serializable]
public class PlayerTorch : UsableItem
{
    private Torch torch;
    ItemInfo itemInfo;

    public override void Start()
    {
        //itemInfo = DataManager.UserData.equipUseableItem[1];
        torch = InGame.PlayerBase.GetComponentInChildren<Torch>();
        torch.ChangeTorchLight(false);
    }

    public override void Use()
    {
        torch.ChangeTorchLight(!torch.TorchLight.enabled);
    }
}
