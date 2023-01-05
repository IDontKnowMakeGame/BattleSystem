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
            if (adrenalineSlider is null)
                return;
            if(adrenalineFillArea is null)
                return;
            if(angerSlider is null)
                return;
            if(angerFillArea is null)
                return;
                // FillArea Check
            if (angerPercent == 0 && angerFillArea.activeSelf)
                angerFillArea.SetActive(false);
            else if (angerPercent > 0 && !angerFillArea.activeSelf)
                angerFillArea.SetActive(true);

            if (adrenalinePercent == 0 && adrenalineFillArea.activeSelf)
                adrenalineFillArea.SetActive(false);
            else if (adrenalinePercent > 0 && !adrenalineFillArea.activeSelf)
                adrenalineFillArea.SetActive(true);


            angerSlider.value = angerPercent;
            adrenalineSlider.value = adrenalinePercent;
        }
    }
}
