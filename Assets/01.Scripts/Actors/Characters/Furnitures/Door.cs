using Core;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Actors.Characters.Furnitures
{
    public class Door : Furniture
    {
        private Transform anchorTrm;
        private bool isOpening = false;
        protected override void Awake()
        {
            OnInteract = () =>
            {
                Open();
            };
            DeInteract = () =>
            {
                Close();
            };

            anchorTrm = transform.GetChild(0);
            base.Awake();
        }

        protected override void Start()
        {
            Define.GetManager<DataManager>().AddItemInInventory(ItemID.TEST);
            base.Start();
        }

        public void Open()
        {
            if(isOpening) return;
            isOpening = true;
            anchorTrm.DOLocalRotate(new Vector3(0, -90, 0), 1f).OnComplete(() =>
            {
                IsUpdatingPosition = false;
                isOpening = false;
                var block = InGame.GetBlock(Position);
                block.SetActorOnBlock(null);
            });
        }
        
        public void Close()
        {
            if (isOpening) return;
            isOpening = true;
            IsUpdatingPosition = true;
            InGame.SetActorOnBlock(this);
            anchorTrm.DOLocalRotate(new Vector3(0, 0, 0), 1f).OnComplete(() =>
            {
                isOpening = false;
            });
        }
    }
}