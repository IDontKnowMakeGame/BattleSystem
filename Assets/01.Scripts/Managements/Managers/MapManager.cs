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
            _map.Add(block.Position, block);
        }

        public BlockBase GetBlock(Vector3 pos)
        {
            pos.y = 0;
            return _map[pos];
        }

        public List<BlockBase> GetNeighbors(BlockBase tile)
        {
            List<BlockBase> neighbors = new List<BlockBase>();
            int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

            for (int i = 0; i < 4; i++)
            {
                int checkX = tile.X + temp[i, 0];
                int checkZ = tile.Z + temp[i, 1];

                Vector3 checkPos = new Vector3(checkX, 0, checkZ);

                if (_map.ContainsKey(checkPos))
                {
                    neighbors.Add(_map[checkPos]);
                }
            }

            return neighbors;
        }
    }
}