using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BloodController : MonoBehaviour
{
    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private Image blood;

    private void Awake()
    {
        blood.gameObject.SetActive(false);
    }

    public void StartBlood()
    {
        DOTween.Kill(blood);
        blood.DOFade(0.5f, 0.1f).OnComplete(() =>
        {
            blood.gameObject.SetActive(true);

            blood.DOFade(0, fadeTime).OnComplete(() =>
            {
                blood.gameObject.SetActive(false);
            });
        });
    }
}
