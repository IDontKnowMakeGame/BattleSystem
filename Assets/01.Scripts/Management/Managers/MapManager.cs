using System.Collections.Generic;
using Blocks;
using UnityEngine;

namespace Managements.Managers
{
    public class MapManager : Manager
    {
        private Dictionary<Vector3, Block> _mapDict = new();

        public void AddBlock(Vector3 pos, Block block)
        {
            _mapDict.Add(pos, block);
        }

        public Block GetBlock(Vector3 pos)
        {
            return _mapDict[pos];
        }
    }
}