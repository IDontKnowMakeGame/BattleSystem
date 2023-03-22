using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.NPC;
using Actors.Bases;
using Acts.Characters;
using Core;
using Managements.Managers;

namespace Actors.Characters.NPC
{
    public class NPCActor : CharacterActor
    {
        [SerializeField] 
        private NPCAnimation _npcAnimation;

        protected override void Init()
        {
            base.Init();
            AddAct<CharacterRender>();
            AddAct(_npcAnimation);

            InputManager<Weapon>.OnInteractionPress += Interact;

        }

        public virtual void Interact()
        {
            if (InGame.Player.Position.IsNeighbor(Position) == false) return;
            
            //TODO : 상호작용
        }
    }
}
