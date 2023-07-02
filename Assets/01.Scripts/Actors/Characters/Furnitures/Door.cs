using Core;
using Data;
using DG.Tweening;
using UnityEngine;

namespace Actors.Characters.Furnitures
{
    public class Door : Furniture
    {
        [SerializeField]
        private DialogueData dialogueData;
        private Transform anchorTrm;
        private bool isOpening = false;
        protected override void Awake()
        {
            OnInteract = () =>
            {
                Open();
				Define.GetManager<SoundManager>().PlayAtPoint("Sounds/Effect/IronDoor", InGame.Player.Position);
			};
            DeInteract = () =>
            {
                Close();
            };
            FaildInteract = () =>
            {
                Faild();
				Define.GetManager<SoundManager>().PlayAtPoint("Sounds/Effect/DoorLock", InGame.Player.Position);
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
                RemoveInteration();
                return;
            }

            base.Start();
        }

        public void Faild()
        {
            UIManager.Instance.Dialog.StartListeningDialog(dialogueData,true);
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