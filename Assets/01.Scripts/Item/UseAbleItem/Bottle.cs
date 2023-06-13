using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[System.Serializable]
public class Bottle : UseAbleItem
{
    public override bool UseItem()
    {
        GameObject torch = Define.GetManager<ResourceManager>().Instantiate("Bottle");
        torch.GetComponent<MolotovCocktail>().InitBottle(Weapon.DirReturn(Input.mousePosition));
        torch.GetComponent<MolotovCocktail>().isPlay = true;
        //방향을 알고 그 방향의 병 생성
        return true;
    }
}
