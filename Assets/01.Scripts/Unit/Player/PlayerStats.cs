using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unit;
using UnityEngine.UI;
using Manager;

namespace Unit.Player
{
    [System.Serializable]
    public class PlayerStats : UnitStat
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


        private InputManager _testInputManager;
        public override void Start()
        {
            _testInputManager = GameManagement.Instance.GetManager<InputManager>();
        }

        public override void Update()
        {
            if (_testInputManager.GetKeyDownInput(InputManager.InputSignal.TestHit))
                Damage(5);

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


            angerSlider.value = angerPercent;
            adrenalineSlider.value = adrenalinePercent;
        }
    }
}
