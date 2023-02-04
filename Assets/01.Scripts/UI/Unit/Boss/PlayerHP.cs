using System.Collections;
using System.Collections.Generic;
using Core;
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
        Define.GetManager<EventManager>().StartListening(EventFlag.AddPlayerHP, AddPlayerHP);
    }
    private void AddPlayerHP(EventParam value)
    {
        _slider.value = value.floatParam;
    }

    private void OnDestroy()
    {
        Define.GetManager<EventManager>().StopListening(EventFlag.AddPlayerHP, AddPlayerHP);
    }

    private void OnApplicationQuit()
    {
        Define.GetManager<EventManager>().StopListening(EventFlag.AddPlayerHP, AddPlayerHP);
    }
}
