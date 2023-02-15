using Managements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderObject : MonoBehaviour
{
	[SerializeField]
	private float _maxYScalevalue;

	private float _maxChargeValue;
	private float _chargeValue;
	private float _addValue;

	private List<GameObject> _sliders = new List<GameObject>();
	private GameObject _slider;

	private Action _action;

	private Vector3 firstSliderScale = new Vector3(1, 0, 1);
	private void Start()
	{
		_sliders = this.gameObject.transform.AllChildrenObjList();
		_slider = _sliders.Find((a) => a.name == "Slider");
		ActiveObjects(false);

		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.SliderInit, SliderInit);
		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.SliderUp, SliderUp);
		GameManagement.Instance.GetManager<EventManager>().StartListening(EventFlag.SliderFalse, SliderActiveFalse);
	}
	public void SliderInit(EventParam eventParam)
	{
		_maxChargeValue = eventParam.floatParam;
		_addValue = _maxYScalevalue / _maxChargeValue;
		_slider.transform.localScale = firstSliderScale;
		_action = eventParam.actionParam;
		ActiveObjects(true);
	}
	public void SliderUp(EventParam eventParam)
	{
		if(_slider.transform.localScale.y >= _maxYScalevalue)
		{
			_action?.Invoke();
			_slider.transform.localScale = firstSliderScale;
		}
		else
		{
			_chargeValue += _addValue;
		}
	}

	public void SliderActiveFalse(EventParam eventParam) => ActiveObjects(false);

	private void ActiveObjects(bool isActive)
	{
		foreach(var a in _sliders)
		{
			a.SetActive(isActive);
		}
	}
}
