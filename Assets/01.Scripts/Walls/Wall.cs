using Actors.Bases;
using UnityEngine;
using Walls.Acts;

namespace Walls
{
    public class Wall : Actor
    {
        [SerializeField] private WallRender wallRender;
        protected override void Init()
        {
            AddAct(wallRender);
            base.Init();
        }
    }
}