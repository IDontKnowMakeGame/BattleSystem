using System.Collections.Generic;
using UnityEngine;
using System;

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
    CameraShake
}
public class EventManager
{
    private static Dictionary<EventFlag, Action<EventParam>> eventDictionary = new Dictionary<EventFlag, Action<EventParam>>();

    public static void StartListening(EventFlag eventName, Action<EventParam> listener)
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

    public static void StopListening(EventFlag eventName, Action<EventParam> listener)
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

    public static void TriggerEvent(EventFlag eventName, EventParam eventParam)
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
}

