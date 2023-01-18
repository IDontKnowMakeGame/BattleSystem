using System.Collections.Generic;
using Managements.Managers.Base;
using UnityEngine;

namespace Managements.Managers
{
    public class MapManager : Manager
    {
        private Dictionary<Vector3, BlockBase> _map = new();
        
        public void AddBlock(BlockBase block)
        {
            _map.Add(block.transform.position, block);
        }
    }
}