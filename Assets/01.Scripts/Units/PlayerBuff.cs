using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Behaviours.Unit;
using Unit.Core;
using Core;

namespace Units.Base.Player
{
    [System.Serializable]
    public class PlayerBuff : UnitBehaviour
    {
        [SerializeField]
        [Range(0, 10)] private float anger;
        [SerializeField]
        [Range(0, 10)] private float adneraline;
        [SerializeField]
        private ParticleSystem angerParticle;
        [SerializeField]
        private ParticleSystem adneralineParticle;

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

        private PlayerStat _playerStat;
		public override void Start()
        {
            _playerStat = ThisBase.GetBehaviour<PlayerStat>();

            attackCount = 0;
            attackCheckTimer = attckCheckTime;

            if(angerParticle != null)
                angerParticle.gameObject.SetActive(false);
            if(adneralineParticle != null)
                adneralineParticle.gameObject.SetActive(false);


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
            Core.Define.GetManager<UIManager>().SetAngerValue((int)anger*10);
            //Define.GetManager<EventManager>().TriggerEvent(EventFlag.AddAnger, eventParam);
        }

        public void ChangeAdneraline(float percent)
        {
            if (percent > 0)
                attackCount++;
            adneraline = Mathf.Clamp(adneraline + percent, 0, 10);
            Core.Define.GetManager<UIManager>().SetAdranalineValue((int)adneraline * 10);

        }

        private void DecreaseAnger()
        {
            if(anger >= 10 && !angerDecrease)
            {
                angerDecrease = true;
                decreaseAngerTimer = decreaseTime;
                _playerStat.multistat.Atk += 1;
                ThisBase.GetBehaviour<PlayerStat>().Half += 50;
                angerParticle.gameObject.SetActive(true);
            }
            if (angerDecrease)
            {
                if (anger <= 0)
                {
                    anger = 0;
                    angerDecrease = false;
                    _playerStat.multistat.Atk -= 1;
                    ThisBase.GetBehaviour<PlayerStat>().Half -= 50;
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

            if(attackCheckTimer <= 0)
            {
                if (attackCount == 0)
                    ChangeAdneraline(-decreseAttackCheckPercent);

                attackCount = 0;
                attackCheckTimer = attckCheckTime;
            }

            if (adneraline >= 10 && !adneralineDecrease)
            {
                adneralineDecrease = true;
                decreaseAdneralineTimer = decreaseTime;
                _playerStat.multistat.Atk += 0.5f;
                _playerStat.addstat.Agi += 1;
                adneralineParticle.gameObject.SetActive(true);
            }
            if (adneralineDecrease)
            {
                if (adneraline <= 0)
                {
                    adneraline = 0;
                    adneralineDecrease = false;
                    _playerStat.multistat.Atk -= 0.5f;
                    _playerStat.addstat.Agi -= 1;
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
