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
        EventManager.StartListening(EventFlag.ShowBossHP, ShowBossHP);
        EventManager.StartListening(EventFlag.AddBossHP, AddBossHP);

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
        EventManager.StopListening(EventFlag.ShowBossHP, ShowBossHP);
        EventManager.StopListening(EventFlag.AddBossHP, AddBossHP);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EventFlag.ShowBossHP, ShowBossHP);
        EventManager.StopListening(EventFlag.AddBossHP, AddBossHP);
    }
}
