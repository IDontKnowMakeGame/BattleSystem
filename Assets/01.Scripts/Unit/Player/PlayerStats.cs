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

        [SerializeField]
        private BasicHPSlider _basicHPSlider;

		public override void Start()
        {
            _testInputManager = GameManagement.Instance.GetManager<InputManager>();
            _basicHPSlider.InitSlider(GetOriginalStat().hp);
            base.Start();
        }

        public override void Update()
        {
            if (_testInputManager.GetKeyDownInput(InputManager.InputSignal.TestHit))
                Damaged(5);

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

        public void AddAngerPercent(int percent)
        {
            angerPercent = Mathf.Clamp(angerPercent + percent, 0, 10);
        }

        public void AddAdrenaline(int percent)
        {
            adrenalinePercent = Mathf.Clamp(adrenalinePercent + percent, 0, 10);
        }

        public void SetAngerPercent(int percent)
        {
            angerPercent = Mathf.Clamp(percent, 0, 10);
        }

        public void SetAdrenaline(int percent)
        {
            adrenalinePercent = Mathf.Clamp(percent, 0, 10);
        }

		public override void Damaged(float damage)
		{
            GetCurrentStat().hp -= damage;
            _basicHPSlider.SetSlider(GetCurrentStat().hp);
            if (GetCurrentStat().hp <= 0)
			{
                Die();
            }
		}

        public override void Die()
		{

		}
	}
}
