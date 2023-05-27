using Core;
using Managements.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
public enum ScreenEffectEnum
{
    None,
    Speed = 0,
}

public class ScreenEffects : MonoBehaviour
{
    [SerializeField]
    private VisualEffectAsset[] _vegs;

    [SerializeField]
    private VisualEffect _visualEffect;

    private ScreenEffectEnum vegEnum;

    public ScreenEffectEnum VEG => vegEnum;

	private void Start()
	{
        StopEffect();
        Define.GetManager<EventManager>().StartListening(EventFlag.PlayScreenEffect, PlayEffect);
        Define.GetManager<EventManager>().StartListening(EventFlag.StopScreenEffect, StopEffect);
	}

    private void PlayEffect(EventParam events)
    {
        PlayEffect((ScreenEffectEnum)events.intParam);
	}

	private void StopEffect(EventParam events)
	{
		StopEffect();
	}

    public void PlaySpeedEffect() => PlayEffect(ScreenEffectEnum.Speed);

	public void PlayEffect(ScreenEffectEnum effect)
    {
        vegEnum = effect;
        _visualEffect.visualEffectAsset = _vegs[(int)vegEnum];
        _visualEffect.Play();
		_visualEffect.gameObject.SetActive(true);
	}

	public void StopEffect() 
    {
        vegEnum = ScreenEffectEnum.None;
		_visualEffect.Stop();
		_visualEffect.gameObject.SetActive(false);
	}
}
