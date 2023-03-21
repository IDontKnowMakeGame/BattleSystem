using System.Collections.Generic;
using Actors.Bases;
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
            if (!_mapDict.ContainsKey(pos))
                return null;
            return _mapDict[pos];
        }
        
        public void AttackBlock(Vector3 pos, float damage, float delay, Actor attacker)
        {
            if (!_mapDict.ContainsKey(pos))
                return;
            _mapDict[pos].Attack(damage, Color.red, delay, attacker);
        }

        public List<Block> GetNeighbors(Block tile)
        {
            List<Block> neighbors = new List<Block>();
            int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

            for (int i = 0; i < 4; i++)
            {
                int checkX = tile.X + temp[i, 0];
                int checkZ = tile.Z + temp[i, 1];

                Vector3 checkPos = new Vector3(checkX, 0, checkZ);

                if (_mapDict.ContainsKey(checkPos))
                {
                    neighbors.Add(_mapDict[checkPos]);
                }
            }

            return neighbors;
        }
    }
}