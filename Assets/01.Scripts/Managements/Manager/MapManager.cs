using System.Collections.Generic;
using Actor.Bases;
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
        
        public BlockController GetBlock(ActorController actor)
        {
            return BlockDictionary[actor.Position];
        }

        public List<BlockController> GetNeighbors(BlockController tile)
        {
            List<BlockController> neighbors = new List<BlockController>();
            int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

            for (int i = 0; i < 4; i++)
            {
                int checkX = tile.X + temp[i, 0];
                int checkZ = tile.Z + temp[i, 1];

                Vector3 checkPos = new Vector3(checkX, 0, checkZ);

                if (BlockDictionary.ContainsKey(checkPos))
                {
                    neighbors.Add(BlockDictionary[checkPos]);
                }
            }

            return neighbors;
        }
    }
}