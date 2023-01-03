using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using UnityEngine.UI;

namespace Unit.Player
{
    [System.Serializable]
    public class PlayerStats : Behaviour
    {
        [Header("Percent")]
        [Range(0, 10)]
        [SerializeField] private int angerPercent;
        [Range(0, 10)]
        [SerializeField] private int adrenalinePercent;
        [Header("UI_Slider")]
        [SerializeField]
        private Slider angerSlider;
        [SerializeField]
        private GameObject angerFillArea;
        [SerializeField]
        private Slider adrenalineSlider;
        [SerializeField]
        private GameObject adrenalineFillArea;
        public override void Start()
        {
           
        }

        public override void Update()
        {
            ChangeStatsUI();
        }

        private void ChangeStatsUI()
        {
            // FillArea Check
            if (angerPercent == 0 && angerFillArea.activeSelf)
                angerFillArea.SetActive(false);
            else if (angerPercent > 0 && !angerFillArea.activeSelf)
                angerFillArea.SetActive(true);


            angerSlider.value = angerPercent;
            adrenalineSlider.value = adrenalinePercent;
        }
    }
}
