using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Data;
using Managements.Managers;
using UnityEngine;

namespace Actors.Characters.Furnitures
{
    public class Furniture : CharacterActor
    {
        public List<ItemID> NeedItems = new ();
        private bool IsInteracting = false; 
        protected Action OnInteract = null;
        protected Action DeInteract = null;
        
        public void Interact()
        {
            if (InGame.Player.Position.IsNeighbor(Position) == false) return;
            Debug.Log("near");
            if (IsInteracting == false)
            {
                var needItems = NeedItems.Where((x) => !DataManager.HaveQuestItem(x)).ToList();

                Debug.Log(needItems.Count);
                if(needItems.Count > 0) return;
                
                OnInteract?.Invoke();
                IsInteracting = true;
            }
            else
            {
                DeInteract?.Invoke();
                IsInteracting = false;
            }
        }

        protected override void Awake()
        {
            InputManager<Weapon>.OnInteractionPress += Interact;
            base.Awake();
        }
    }
}