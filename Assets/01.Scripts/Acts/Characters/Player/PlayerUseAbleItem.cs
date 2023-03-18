using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Base;
using Data;

namespace Acts.Characters.Player
{
    [System.Serializable]
    public class PlayerUseAbleItem : Act
    {
        [Header("Torch")]
        [SerializeField]
        private GameObject torch;
        [Header("Shield")]
        [SerializeField]
        private GameObject shield;
        public GameObject Torch => torch;
        public GameObject Shield => shield;

        private Torch torchItem;
        private Shield shieldItem;

        //private Dictionary<ItemID, UseAbleItem> useAbleItems;



        public override void Start()
        {
            base.Start();

            //useAbleItems.Add(ItemID.Torch, new Torch(this));
            //useAbleItems.Add(ItemID.Shield, new Shield(this));
        }

        public override void Update()
        {
            base.Update();

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                torchItem.UseItem();
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                shieldItem.UseItem();
            }
        }
    }
}
