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

    protected override void Use()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            torch.ChangeTorchLight(!torch.TorchLight.enabled);
        }
    }
}
