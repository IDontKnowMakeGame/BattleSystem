using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    void Start()
    {
        EventManager.StartListening(EventFlag.AddPlayerHP, AddPlayerHP);
    }
    private void AddPlayerHP(EventParam value)
    {
        _slider.value = value.floatParam;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventFlag.AddPlayerHP, AddPlayerHP);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EventFlag.AddPlayerHP, AddPlayerHP);
    }
}
