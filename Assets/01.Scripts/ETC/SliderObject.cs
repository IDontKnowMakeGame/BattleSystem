using Managements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SliderObject : MonoBehaviour
{
	[SerializeField]
	private float _maxYScalevalue;

	private float _maxChargeValue;
	private float _chargeValue;
	private float _addValue;

	private bool _isPull;
	private float _power;
	private Color _color = Color.white;

	private List<GameObject> _sliders = new List<GameObject>();
	private GameObject _slider;
	private GameObject _backGroundObject;

	private Vector3 firstSliderScale = new Vector3(1, 0, 1);
	private Vector3 vec = new Vector3(1, 0, 1);
	private void Start()
	{
		_sliders = this.gameObject.transform.AllChildrenObjList();
		_slider = _sliders.Find((a) => a.name == "Slider");
		_backGroundObject = _sliders.Find((a) => a.name == "BackGround");
		ActiveObjects(false);

		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.SliderInit, SliderInit);
		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.SliderUp, SliderUp);
		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.SliderFalse, SliderActive);
		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.PullSlider, PullSlider);
	}

	private void Update()
	{
		PullAnimation(_power, _color);
	}
	private void SliderInit(EventParam eventParam)
	{
		_maxChargeValue = eventParam.floatParam;
		_slider.transform.localScale = firstSliderScale;
		ActiveObjects(true);
	}
	private void SliderUp(EventParam eventParam)
	{
		if (_slider.transform.localScale.y >= _maxYScalevalue)
		{
			_slider.transform.localScale = firstSliderScale;
		}
		else
		{
			_chargeValue = (eventParam.floatParam / _maxChargeValue) * 100f;

			if (float.IsInfinity(_chargeValue))
				_chargeValue = 0;

			vec.y = _maxYScalevalue * Mathf.Floor(_chargeValue) / 100f;
			_slider.transform.localScale = vec;
		}
	}

	private void SliderActive(EventParam eventParam) => ActiveObjects(eventParam.boolParam);

	private void PullSlider(EventParam eventParam)
	{
		_isPull = eventParam.boolParam;
		_power = eventParam.floatParam;
		//_color = eventParam.color;
	}

	private void ActiveObjects(bool isActive)
	{
		foreach (var a in _sliders)
		{
			a.SetActive(isActive);
		}
	}

	private void PullAnimation(float power, Color color)
	{
		if (_isPull)
		{
			_backGroundObject.GetComponent<SpriteRenderer>().color = color;
		}
	}
}