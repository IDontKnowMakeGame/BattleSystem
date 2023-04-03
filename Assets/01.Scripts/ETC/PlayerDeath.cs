using System;
using Core;
using DG.Tweening;
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
            InGame.Player.enabled = false;
            Sequence seq = DOTween.Sequence();
            vignette.intensity.value = 1f;
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 2f);
            seq.AppendInterval(4f);
            seq.AppendCallback(() =>
            {
                InGame.Player.StopAllCoroutines();
                SceneManager.LoadScene("Lobby");
                DOTween.KillAll();
            });
        }
    }
}