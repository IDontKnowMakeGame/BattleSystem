using System.Collections.Generic;
using Block.Base;
using UnityEngine;

namespace Managements.Managers
{
    public class MapManager : Manager
    {
        public Dictionary<Vector3, BlockController> BlockDictionary { get; private set; } = new();
        
        public BlockController GetBlock(Vector3 pos)
        {
            return BlockDictionary[pos];
        }
    }
}