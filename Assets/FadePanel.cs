using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : ScreenEffectObject
{
	private Image _image;

	private void FadeEffect(float speed, float value)
	{
		_image.DOFade(speed, value);
	}

	private void Awake()
	{
		_image=GetComponent<Image>();
		ScreenEffect.Fade += FadeEffect;
	}
}
