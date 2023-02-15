using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class SliderUI : MonoBehaviour
{
    protected Slider _slider;
    protected EventFlag addflag;

    public virtual void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public virtual void Start()
    {
        Define.GetManager<EventManager>().StartListening(addflag, AddHP);
    }

    public virtual void AddHP(EventParam value)
    {
        _slider.value = value.floatParam;
    }

    public virtual void OnDestroy()
    { 
        var manager = Define.GetManager<EventManager>();
        manager?.StopListening(addflag, AddHP);
    }

    public virtual void OnApplicationQuit()
    {
        var manager = Define.GetManager<EventManager>();
        manager?.StopListening(addflag, AddHP);
    }
}
