using Actors.Characters.NPC;
using DG.Tweening;
using UnityEngine;

namespace Actors
{
    public class RisingDoor : TouchActor
    {
        [SerializeField] private Transform risingTrm;
        [SerializeField] private Light light = null;
        [SerializeField] private bool canOpen = false;


        protected override void Update()
        {
            if(canOpen)
                base.Update();
        }

        public void Rise()
        {
            light.DOIntensity(5, 3).OnComplete(() =>
            {
                canOpen = true;
            });
        }
    }
}