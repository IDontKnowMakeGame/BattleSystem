using System;
using Core;
using DG.Tweening;
using Managements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace ETC
{
    public class PlayerDeath : MonoBehaviour
    {
        public Volume volume;
        private Vignette vignette;

        private static PlayerDeath instance;
        public static PlayerDeath Instance => instance;

        private void Awake()
        {
            instance = this;
            volume.profile.TryGet<Vignette>(out vignette);
        }

        public void FocusCenter()
        {
            //InGame.Player.StopAllCoroutines();
            //Destroy(InGame.Player.gameObject);
            //DOTween.KillAll();
            Sequence seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.8f, 1.5f).SetEase(Ease.Linear));
            seq.AppendCallback(() =>
            {
                LoadingSceneController.Instnace.StartCoroutine(LoadingSceneController.Instnace.LoadScene("Lobby", 1f));
                DOTween.CompleteAll();
                DOTween.KillAll();
                DOTween.Kill(seq);
            });
            seq.Play();
        }
    }
}