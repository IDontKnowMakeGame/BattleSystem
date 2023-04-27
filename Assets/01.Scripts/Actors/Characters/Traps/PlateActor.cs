using System.Collections.Generic;
using Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Actors.Characters.Traps
{
    public class PlateActor : TrapActor
    {
        [SerializeField] private List<TrapActor> trapActors = new List<TrapActor>();
        protected override void Start()
        {
            OnTrapActiveCondition += () =>
            {
                var block = InGame.GetBlock(Position);
                if (block == null)OnTrapActiveCondition += () =>
                {
                    var block = InGame.GetBlock(Position);
                    if (block == null)
                        return false;
                    if (block.ActorOnBlock == null)
                        return false;
                    return true;
                };
                    return false;
                if (block.ActorOnBlock == null)
                    return false;
                return true;
            };
            foreach (var trapActor in trapActors)
            {
                trapActor.OnTrapActiveCondition = () => IsTrapInput;
            }
            base.Awake();
        }
    }
}