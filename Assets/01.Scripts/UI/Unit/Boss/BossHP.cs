using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Managements.Managers;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();     
    }
    void Start()
    {
        Define.GetManager<EventManager>().StartListening(EventFlag.ShowBossHP, ShowBossHP);
        Define.GetManager<EventManager>().StartListening(EventFlag.AddBossHP, AddBossHP);

        _slider.gameObject.SetActive(false);
    }

    private void ShowBossHP(EventParam value)
    {
        _slider.gameObject.SetActive(true);
    }
    private void AddBossHP(EventParam value)
    {
        _slider.value = value.floatParam;
    }

    private void OnDestroy()
    {
        Define.GetManager<EventManager>().StopListening(EventFlag.ShowBossHP, ShowBossHP);
        Define.GetManager<EventManager>().StopListening(EventFlag.AddBossHP, AddBossHP);
    }

    private void OnApplicationQuit()
    {
        Define.GetManager<EventManager>().StopListening(EventFlag.ShowBossHP, ShowBossHP);
        Define.GetManager<EventManager>().StopListening(EventFlag.AddBossHP, AddBossHP);
    }
}
