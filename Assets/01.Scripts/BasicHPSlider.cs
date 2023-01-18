using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicHPSlider : MonoBehaviour
{
	[Header("HP Slider")]
	[SerializeField]
	private Slider BasicSlider;
	[SerializeField]
	private Slider whiteSlider;

	[SerializeField] private float sliderSpeed;

	private bool isDamage = false;

	private bool reverse = false;
	private void Update()
	{
		UpdateSlider();
	}

	public void InitSlider(float max, float value = -1)
	{
		BasicSlider.maxValue = max;
		BasicSlider.value = value != -1 ? value : max;

		whiteSlider.maxValue = max;
		whiteSlider.value = value != -1 ? value : max;
	}
	public void SetSlider(float hp, bool reverse = false)
	{
		this.reverse = reverse;
		if (!reverse)
			BasicSlider.value = hp;
		else
			whiteSlider.value = hp;
		isDamage = true;
	}

	private void UpdateSlider()
	{
		if (isDamage)
		{
			if (!reverse)
				whiteSlider.value = Mathf.Lerp(whiteSlider.value, BasicSlider.value, Time.deltaTime * sliderSpeed);
			else
				BasicSlider.value = Mathf.Lerp(BasicSlider.value, whiteSlider.value, Time.deltaTime * sliderSpeed);
		}
	}
}
