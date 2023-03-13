using System;
using Actor.Bases;
using ControllerBase;
using Core;
using Managements.Managers;
using UnityEngine;

namespace Block.Base
{
    public class BlockController : Controller
    {

        [SerializeField] private ActorController _actorOnBlock;

        private void Awake()
        {
            Position = transform.position;
            Define.GetManager<MapManager>().BlockDictionary.Add(Position, this);
        }

        public void SetActorOnBlock(ActorController actor)
        {
            _actorOnBlock = actor;
        }
        
        public ActorController GetActorOnBlock()
        {
            return _actorOnBlock;
        }
    }
}