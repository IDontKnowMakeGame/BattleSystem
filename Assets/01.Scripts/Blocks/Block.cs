using Actors.Bases;
using Blocks.Acts;
using Core;
using UnityEngine;

namespace Blocks
{
    public class Block : Actor
    {
        [SerializeField] private Actor _actorOnBlock;
        public Actor ActorOnBlock => _actorOnBlock;
        protected override void Init()
        {
            AddAct<BlockRender>();
        }

        protected override void Awake()
        {
            Position = transform.position;
            InGame.AddBlock(this);
            base.Awake();
        }

        public void SetActorOnBlock(Actor actor)
        {
            _actorOnBlock = actor;
        }
        
        public void RemoveActorOnBlock()
        {
            _actorOnBlock = null;
        }
    }
}