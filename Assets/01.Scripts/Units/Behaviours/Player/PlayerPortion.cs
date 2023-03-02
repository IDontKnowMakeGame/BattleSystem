using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Player;
using Core;

[System.Serializable]
public class PlayerPortion : UsableItem
{
    private int cnt;

    private float timer = 0;

    private bool hp = false;

    public bool UsePortion => hp;

    GameObject starParticle;

    ItemInfo itemInfo;

    public override void Start()
    {
        itemCnt = 10;
        //itemInfo = DataManager.UserData.equipUseableItem[0];
        ResetPortion();
    }

    public void DecreaseHPortion()
    {
        if (cnt >= 100 || InGame.PlayerBase.GetBehaviour<PlayerStat>().OriginStats.Hp <= InGame.PlayerBase.GetBehaviour<PlayerStat>().NowStats.Hp)
        {
            ResetPortion();
            return;
        }

        timer += Time.deltaTime;

        if(timer >= 0.015f)
        {
            timer = 0;
            InGame.PlayerBase.GetBehaviour<PlayerStat>().AddHP(1);
            cnt++;
        }
    }

    public void ResetPortion()
    {
        Core.Define.GetManager<ResourceManagers>().Destroy(starParticle);
        hp = false;
        cnt = 0;
        timer = 0;
        timer = 0;
    }

    protected override void Use()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !hp)
        {
            hp = true;
            starParticle = Core.Define.GetManager<ResourceManagers>().Instantiate("Star_A");
            starParticle.transform.position = InGame.PlayerBase.Position;
            itemCnt--;
        }
        if (hp)
            DecreaseHPortion();
    }
}
