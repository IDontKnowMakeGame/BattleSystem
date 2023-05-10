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
            Sequence seq = DOTween.Sequence();
            vignette.intensity.value = 0;
            vignette.rounded.value = true;
            seq.Append(DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 1f));
            seq.AppendInterval(2f);
            seq.AppendCallback(() =>
            {
                DOTween.CompleteAll();
                SceneManager.LoadScene("Lobby");
                DOTween.KillAll();
                DOTween.Kill(seq);
            });
        }
    }
}