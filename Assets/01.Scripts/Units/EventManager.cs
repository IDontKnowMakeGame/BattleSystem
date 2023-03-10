using System.Collections.Generic;
using UnityEngine;
using System;
using Managements.Managers.Base;
using Units.Base.Unit;

public enum EventFlag
{
    ShowBossHP,
    AddBossHP,
    ShowPlayerHP,
    AddPlayerHP,
    WeaponEquip,
    WeaponUnmount,
    WeaponSwap,
    WeaponChange,
    CameraShake,
    ShowInterection,
    HideInterection,
    WeaponPanelConnecting,
    WeaponPanelDisConnecting,
    AddAnger,
    AddAdrenaline,
    PlayTimeLine,
    DirtyHalo,
    SliderInit,
    SliderUp,
    SliderFalse,
    HPWidth,
    PullSlider,
    WeaponUpgrade,
    SetWeapon,
    UnsetWeapon
}
public class EventManager : Manager
{
    private Dictionary<EventFlag, Action<EventParam>> eventDictionary = new Dictionary<EventFlag, Action<EventParam>>();

    public void StartListening(EventFlag eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public void StopListening(EventFlag eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public void TriggerEvent(EventFlag eventName, EventParam eventParam)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(eventParam);
        }
    }
}
public struct EventParam
{
    public Vector2Int vectorParam;
    public string stringParam;
    public float floatParam;
    public int intParam;
    public bool boolParam;
    public Action actionParam;
    public UnitBase unitParam;
    public Color color;
}
