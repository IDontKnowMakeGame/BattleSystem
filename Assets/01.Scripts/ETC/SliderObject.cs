using Managements;
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

	private Vector3 _originPos;
	private bool _isPull;
	private float _power;
	private Color _color = Color.white;

	private List<GameObject> _sliders = new List<GameObject>();
	private GameObject _slider;
	private GameObject _backGroundObject;

	private Vector3 firstSliderScale = new Vector3(0, 0, 1);
	private Vector3 vec = new Vector3(0.5f, 0, 1);
	private void Start()
	{
		_sliders = this.gameObject.transform.AllChildrenObjList();
		_slider = _sliders.Find((a) => a.name == "Slider");
		_backGroundObject = _sliders.Find((a) => a.name == "BackGround");
		_originPos = this.transform.localPosition;
		ActiveObjects(false);
	}

	private void Update()
	{
		PullAnimation();
	}
	public void SliderInit(float maxValue)
	{
		_maxChargeValue = maxValue;
		_slider.transform.localScale = firstSliderScale;
		ActiveObjects(true);
	}
	public void SliderUp(float upValue)
	{
		if (_slider.transform.localScale.y >= _maxYScalevalue)
		{
			_slider.transform.localScale = firstSliderScale;
		}
		else
		{
			_chargeValue = (upValue / _maxChargeValue) * 100f;

			if (float.IsInfinity(_chargeValue))
				_chargeValue = 0;

			vec.y = _maxYScalevalue * Mathf.Floor(_chargeValue) / 100f;
			_slider.transform.localScale = vec;
		}
	}

	public void SliderActive(bool active) => ActiveObjects(active);

	public void PullSlider(float power, bool isPull, Color color)
	{
		_isPull = isPull;
		_power = power;
		_color = color;

		if(_backGroundObject != null) 
		{
			_backGroundObject.GetComponent<SpriteRenderer>().color = _color;
			this.transform.localPosition = _originPos;
		}
	}

	private void ActiveObjects(bool isActive)
	{
		foreach (var a in _sliders)
		{
			a.SetActive(isActive);
		}
	}

	private void PullAnimation()
	{
		if (_isPull)
			transform.localPosition = (Vector3)Random.insideUnitCircle * _power + _originPos;
	}
}