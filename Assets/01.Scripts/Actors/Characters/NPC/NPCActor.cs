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
    public class NPCActor : InteractionActor
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
