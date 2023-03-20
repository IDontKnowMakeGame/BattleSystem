using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acts.Characters.NPC;
using Actors.Bases;

namespace Actors.Characters.NPC
{
    public class NPCActor : CharacterActor
    {
        [SerializeField] 
        private NPCAnimation _npcAnimation;

        protected override void Init()
        {
            base.Init();
            AddAct(_npcAnimation);
        }
    }
}
