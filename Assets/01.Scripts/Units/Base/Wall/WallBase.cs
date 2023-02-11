using UnityEngine;

namespace Units.Base.Wall
{
    public class WallBase : Units
    {
        [SerializeField]
        private WallRender wallRender;
        protected override void Init()
        {
            AddBehaviour(wallRender);
            base.Init();
        }
    }
}