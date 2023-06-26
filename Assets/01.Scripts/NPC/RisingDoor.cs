using Actors.Characters.NPC;
using DG.Tweening;
using UnityEngine;

namespace Scripts.NPC
{
    public class RisingDoor : TouchActor
    {
        [SerializeField] private Transform model = null;
        [SerializeField] private bool canInteract = false;

        protected override void Update()
        {
            if (canInteract == false) return;
            base.Update();
        }

        public void Rise()
        {
            model.DOLocalMoveY(1f, 3).OnComplete(() =>
            {
                canInteract = true;
            });
        }
        
    }
}
