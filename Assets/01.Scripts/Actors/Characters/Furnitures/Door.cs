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
            IsUpdatingPosition = true;
            if (Define.GetManager<DataManager>().IsOpenDoorl(int.Parse(gameObject.name), DataManager.MapData_.currentFloor))
            {
                Open();
            }

            base.Start();
        }

        public void Open()
        {
            if(isOpening) return;
            isOpening = true;

            Define.GetManager<DataManager>().OpenDoor(int.Parse(gameObject.name));

            anchorTrm.DOLocalMoveZ(1.5f, 1f).OnComplete(() =>
            {
                IsUpdatingPosition = false;
                isOpening = false;
                var block = InGame.GetBlock(Position);
                block.SetActorOnBlock(null);
                gameObject.SetActive(false);
            });

        }
        
        public void Close()
        {
            if (isOpening) return;
            gameObject.SetActive(true);
            isOpening = true;
            IsUpdatingPosition = true;
            InGame.SetActorOnBlock(this);
            anchorTrm.DOLocalMoveZ(0.5f, 1f).OnComplete(() =>
            {
                isOpening = false;
            });
        }
    }
}