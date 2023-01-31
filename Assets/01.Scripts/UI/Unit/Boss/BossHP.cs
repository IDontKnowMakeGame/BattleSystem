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

        _slider.gameObject.SetActive(false);
    }
    void Start()
    {
        EventManager.StartListening(EventFlag.ShowBossHP, ShowBossHP);
        EventManager.StartListening(EventFlag.AddBossHP, AddBossHP);
    }

    private void ShowBossHP(EventParam value)
    {
        _slider.gameObject.SetActive(true);
    }
    private void AddBossHP(EventParam value)
    {
        _slider.value = value.floatParam;
    }
}
