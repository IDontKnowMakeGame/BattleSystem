using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

public class BossHP : SliderUI
{
    public override void Awake()
    {
        addflag = EventFlag.AddBossHP;
        Define.GetManager<EventManager>().StartListening(EventFlag.ShowBossHP, ShowBossHP);
        base.Awake();
    }

    private void Start()
    {
        _slider.gameObject.SetActive(false);
    }

    private void ShowBossHP(EventParam value)
    {
        _slider.gameObject.SetActive(true);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        var manager = Define.GetManager<EventManager>();
        manager.StopListening(EventFlag.ShowBossHP, ShowBossHP);
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        var manager = Define.GetManager<EventManager>();
        manager.StopListening(EventFlag.ShowBossHP, ShowBossHP);
    }
}
