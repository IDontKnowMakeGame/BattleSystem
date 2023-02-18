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
    [SerializeField]
    private float waitingTime;
    [SerializeField]
    private float widthMul = 1.5f;

    public RectTransform hpRect;
    private float hittime;
    private bool isDamage = false;

    public override void Awake()
    {
        addflag = EventFlag.AddPlayerHP;
        Define.GetManager<EventManager>().StartListening(EventFlag.HPWidth, SetHpWidth);
        base.Awake();
        hpRect = _slider.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (isDamage)
        {
            hittime += Time.deltaTime;
        }
        UpdateSlider();
    }

    public override void AddHP(EventParam value)
    {
        base.AddHP(value);
        isDamage = true;
    }

    private void UpdateSlider()
    {
        if (hittime > waitingTime && isDamage)
        {
            float whiteHPCheck = Mathf.Lerp(whiteHP.localEulerAngles.x, _slider.value, sliderSpeed * Time.deltaTime);
            whiteHP.localScale = whiteHP.localScale.SetX(whiteHPCheck);
            if (_slider.value >= whiteHPCheck - 0.01f)
            {
                isDamage = false;
                hittime = 0;
                whiteHP.localScale = whiteHP.localScale.SetX(_slider.value);
            }
        }
    }

    private void SetHpWidth(EventParam eventParam)
    {
        hpRect.sizeDelta = hpRect.sizeDelta.SetX(eventParam.floatParam * widthMul);
    }

    public override void OnApplicationQuit()
    {
        Define.GetManager<EventManager>()?.StopListening(EventFlag.HPWidth, SetHpWidth);
        base.OnApplicationQuit();
    }

    public override void OnDestroy()
    {
        Define.GetManager<EventManager>()?.StopListening(EventFlag.HPWidth, SetHpWidth);
        base.OnDestroy();
    }
}
