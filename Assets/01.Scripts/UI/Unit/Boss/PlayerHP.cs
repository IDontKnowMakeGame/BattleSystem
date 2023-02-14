using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : SliderUI
{
    [SerializeField]
    private RectTransform whiteHP;
    [SerializeField]
    private float sliderSpeed;
    private bool isDamage = false;
    public override void Start()
    {
        addflag = EventFlag.AddPlayerHP;
        base.Start();
    }

    private void Update()
    {
        if(isDamage)
        {
            UpdateSlider();
        }
    }

    public override void AddHP(EventParam value)
    {
        base.AddHP(value);
        isDamage = true;
    }

    private void UpdateSlider()
    {
        if (isDamage)
        {
            float whiteHPCheck = Mathf.Lerp(whiteHP.localEulerAngles.x, _slider.value, sliderSpeed * Time.deltaTime);

            whiteHP.localScale = whiteHP.localScale.SetX(whiteHPCheck);
            if (_slider.value >= whiteHPCheck - 0.01f)
            {
                isDamage = false;
                whiteHP.localScale = whiteHP.localScale.SetX(_slider.value);
            }
        }
    }
}
