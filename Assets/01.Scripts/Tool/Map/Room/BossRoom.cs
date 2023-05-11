using Core;
using UnityEngine;

namespace Tool.Map.Rooms
{
    public class BossRoom : Room
    {
        private void Start()
        {
            for (var z = StartPos.z; z <= EndPos.z; z+=1)
            {
                for (var x = StartPos.x; x <= EndPos.x; x+=1)
                {
                    var block = InGame.GetBlock(new Vector3(x, 0, z));
                    block.canBossEnter = true;
                }
            }
        }
    }
}