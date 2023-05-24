using Blocks.Acts;
using UnityEngine;

namespace Blocks
{
    public class FallingBlock : EmptyBlock
    {
        [SerializeField] private float delay = 0.5f;
        private bool isActive = false;
        private float timer = 0.0f;
        private bool isShaking = false;
        private bool isFalling = false;
        private BlockMovement movement = null;
        
        protected override void Awake()
        {
            base.Awake();
            isParent = true;
            movement = GetAct<BlockMovement>();
        }

        protected override void Update()
        {
            if (isActive)
            {
                if (!isShaking)
                {
                    isShaking = true;
                    movement.Shake(delay, 0.1f);
                }
                timer += Time.deltaTime;
                if (timer > delay)
                {
                    if(!isFalling)
                    {
                        isFalling = true;
                        movement.Fall(0.5f);
                    }
                    if(ActorOnBlock == null) return;
                    var stat = ActorOnBlock.GetAct<CharacterStatAct>();
                    stat?.Damage(int.MaxValue, this);
                }
            }
            if(ActorOnBlock == null) return;
            if (ActorOnBlock.Position == Position)
            {
                isActive = true;
            }
            base.Update();
        }
    }
}