using System.Collections.Generic;
using Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors.Characters.Traps
{
    public class PlateActor : TrapActor
    {
        [SerializeField] private List<TrapActor> trapActors = new List<TrapActor>();
        private Transform anchorTrm;
        protected override void Start()
        {
            OnTrapActiveCondition += () =>
            {
                var block = InGame.GetBlock(Position);
                if (block == null)
                    return false;
                if (block.ActorOnBlock == null)
                    return false;
                return true;
            };
            foreach (var trapActor in trapActors)
            {
                if (trapActor == null) continue;
                trapActor.OnTrapActiveCondition = () => IsTrapInput;
            }

            anchorTrm = transform.GetChild(0);
        }

        protected override void Update()
        {
            if(IsTrapInput)
                Press();
            else
            {
                Release();
            }
            base.Update();
        }


        private void Press()
        {
            anchorTrm.localScale = new Vector3(1, 1, 1);
        }
        
        private void Release()
        {
            anchorTrm.localScale = new Vector3(1, 2, 1);
        }
    }
}