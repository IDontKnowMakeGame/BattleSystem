using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Units.Base.Player;

[System.Serializable]
public class PlayerPortion : UnitBehaviour
{
    [SerializeField]
    private int hpPortion = 10;

    private float timer = 0;
    private int cnt = 0;

    private bool hp = false;

    public bool UsePortion => hp;

    GameObject starParticle;

    public override void Start()
    {
        ResetPortion();
        base.Start();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !hp)
        {
            hp = true;
            starParticle = Core.Define.GetManager<ResourceManagers>().Instantiate("Star_A");
            starParticle.transform.position = ThisBase.transform.position;
            hpPortion--;
        }
        if(hp)
            DecreaseHPortion();

        base.Update();
    }

    public void DecreaseHPortion()
    {
        if (cnt >= 100 || ThisBase.GetBehaviour<PlayerStat>().OriginStats.Hp <= ThisBase.GetBehaviour<PlayerStat>().NowStats.Hp)
        {
            ResetPortion();
            return;
        }

        timer += Time.deltaTime;

        if(timer >= 0.015f)
        {
            timer = 0;
            ThisBase.GetBehaviour<PlayerStat>().AddHP(1);
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
}
