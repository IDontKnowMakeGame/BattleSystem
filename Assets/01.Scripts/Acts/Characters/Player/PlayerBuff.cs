using Acts.Base;
using Core;
using UnityEngine;

namespace Acts.Characters.Player
{
    [System.Serializable]
    public class PlayerBuff : Act
    {
        [SerializeField]
        [Range(0, 10)] private float anger;
        [SerializeField]
        [Range(0, 10)] private float adneraline;

        private bool angerDecrease = false;
        private bool adneralineDecrease = false;

        private float decreaseTime = 1f;
        private float decreaseAngerPercent = 1.5f;
        private float decreaseAdneralinePercent = 2f;
        private float decreaseAngerTimer;
        private float decreaseAdneralineTimer;

        private float attckCheckTime = 4f;
        private float decreseAttackCheckPercent = 1f;
        private float attackCheckTimer;
        private int attackCount;

        private EventParam eventParam;

        private CharacterStatAct _playerStat;

        [SerializeField]
        private ParticleSystem angerParticle;
        [SerializeField]
        private ParticleSystem adneralineParticle;
        public override void Start()
        {
            _playerStat = ThisActor.GetAct<CharacterStatAct>();

            attackCount = 0;
            attackCheckTimer = attckCheckTime;

            if (angerParticle != null)
                angerParticle.gameObject.SetActive(false);
            if (adneralineParticle != null)
                adneralineParticle.gameObject.SetActive(false);

            UIManager.Instance.InGame.ChangeAngerValue(0);
            UIManager.Instance.InGame.ChangeAdrenalineValue(0);

            base.Start();
        }

        public override void Update()
        {
            DecreaseAnger();
            DecreaseAdneraline();
            base.Update();
        }

        public void ChangeAnger(float percent)
        {
            anger = Mathf.Clamp(anger + percent, 0, 10);
            UIManager.Instance.InGame.ChangeAngerValue((int)anger * 10);
        }

        public void ChangeAdneraline(float percent)
        {
            if (percent > 0)
                attackCount++;
            adneraline = Mathf.Clamp(adneraline + percent, 0, 10);
            UIManager.Instance.InGame.ChangeAdrenalineValue((int)adneraline * 10);
        }

        private void DecreaseAnger()
        {
            if (anger >= 10 && !angerDecrease)
            {
                if(UIManager.TutorialData_.slider == false)
                {
                    UIManager.TutorialData_.slider = true;
                    UIManager.Instance.SaveToTutorialData();
                    UIManager.Instance.Explanation.Show(8); 
                }
                angerDecrease = true;
                decreaseAngerTimer = decreaseTime;
                _playerStat.AddDrainageAtk("Anger",2);
				_playerStat.Half += 50;
                angerParticle.gameObject.SetActive(true);
				Define.GetManager<SoundManager>().Play("Sounds/Unit/분노", Define.Sound.Effect);
			}
            if (angerDecrease)
            {
                if (anger <= 0)
                {
                    anger = 0;
                    angerDecrease = false;
                    _playerStat.DelDrainageAtk("Anger");
					_playerStat.Half -= 50;
                    angerParticle.gameObject.SetActive(false);
                    return;
                }

                decreaseAngerTimer -= Time.deltaTime;

                if (decreaseAngerTimer <= 0)
                {
                    ChangeAnger(-decreaseAngerPercent);
                    decreaseAngerTimer = decreaseTime;
                }
            }
        }

        private void DecreaseAdneraline()
        {
            attackCheckTimer -= Time.deltaTime;

            if (attackCheckTimer <= 0)
            {
                if (attackCount == 0)
                    ChangeAdneraline(-decreseAttackCheckPercent);

                attackCount = 0;
                attackCheckTimer = attckCheckTime;
            }

            if (adneraline >= 10 && !adneralineDecrease)
            {
                if (UIManager.TutorialData_.slider == false)
                {
                    UIManager.TutorialData_.slider = true;
                    UIManager.Instance.SaveToTutorialData();
                    UIManager.Instance.Explanation.Show(8);
                }
                adneralineDecrease = true;
                decreaseAdneralineTimer = decreaseTime;
                _playerStat.AddDrainageAtk("Adneraline", 1.5f);
                _playerStat.Sub(StatType.Weight, 1f);
                _playerStat.Sub(StatType.ATS, 0.2f);
                adneralineParticle.gameObject.SetActive(true);
				Define.GetManager<SoundManager>().Play("Sounds/Unit/아드레날린", Define.Sound.Effect);
			}
            if (adneralineDecrease)
            {
                if (adneraline <= 0)
                {
                    adneraline = 0;
                    adneralineDecrease = false;
					_playerStat.DelDrainageAtk("Adneraline");
					_playerStat.Plus(StatType.Weight, 1f);
                    _playerStat.Plus(StatType.ATS, 0.2f);
                    adneralineParticle.gameObject.SetActive(false);
                    return;
                }

                decreaseAdneralineTimer -= Time.deltaTime;

                if (decreaseAdneralineTimer <= 0)
                {
                    ChangeAdneraline(-decreaseAdneralinePercent);
                    decreaseAdneralineTimer = decreaseTime;
                }
            }
        }
    }
}
