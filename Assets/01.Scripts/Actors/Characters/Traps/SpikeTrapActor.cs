using System.Collections;
using Core;
using DG.Tweening;
using Managements.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Actors.Characters.Traps
{
    public class SpikeTrapActor : TrapActor
    {
        [SerializeField] private Transform spikeTrm = null; 
        [SerializeField] private float spikeDelay = 0.2f;
        [SerializeField] private int spikeDamage = 30;
        private bool isActive = false;
        protected override void Awake()
        {
            OnTrapActiveCondition = () =>
            {
                var block = InGame.GetBlock(Position);
                if (block == null)
                    return false;
                if (block.ActorOnBlock == null)
                    return false;
                return true;
            };

            OnTrapTrigger += () =>
            {
                if (isActive == false)
                    StartCoroutine(TrapCoroutine());
            };
            base.Awake();
            
            spikeTrm.localScale = new Vector3(1, 0, 1);
        }

        private IEnumerator TrapCoroutine()
        {
            isActive = true;
			Define.GetManager<SoundManager>().PlayAtPoint("Sounds/Effect/Spike", InGame.Player.Position);
			yield return new WaitForSeconds(spikeDelay);
            
            spikeTrm.localScale = Vector3.one;
            var block = InGame.GetBlock(Position);
            if (block != null)
            {
                var actor = block.ActorOnBlock;
                if (actor != null)
                {
                    var stat = actor.GetAct<CharacterStatAct>();
                    stat?.Damage(spikeDamage, this);
                }    
            }
            yield return new WaitForSeconds(spikeDelay * 2f);

            spikeTrm.DOScaleY(0, spikeDelay).onComplete += () => { isActive = false; };
        }
    }
}