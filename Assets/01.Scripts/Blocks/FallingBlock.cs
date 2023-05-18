using UnityEngine;

namespace Blocks
{
    public class FallingBlock : EmptyBlock
    {
        [SerializeField] private float delay = 0.5f;
        private bool isActive = false;
        private float timer = 0.0f;
        protected override void Awake()
        {
            isParent = true;
            base.Awake();
        }

        protected override void Update()
        {
            if (isActive)
            {
                timer += Time.deltaTime;
                if (timer > delay)
                {
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