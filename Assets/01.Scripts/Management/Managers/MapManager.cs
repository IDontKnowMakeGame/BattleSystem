using System.Collections.Generic;
using Actors.Bases;
using Blocks;
using Blocks.Acts;
using UnityEditor.Callbacks;
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
        
        public void AttackBlock(Vector3 pos, float damage, float delay, Actor attacker, MovementType shakeType = MovementType.None,bool isLast = false, float strength = 0.5f)
        {
            if (!_mapDict.ContainsKey(pos))
                return;
            Debug.Log("?");
            _mapDict[pos].Attack(damage, Color.red, delay, attacker, shakeType, isLast, strength);
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
        
        public bool IsWalkable(Vector3 pos)
        {
            if (!_mapDict.ContainsKey(pos))
                return false;
            var tile = _mapDict[pos];
            if (tile == null)
                return false;
            if(tile.isWalkable == false)
                return false;
            return true;
        }
        
        public bool IsCheckFitBlock(Vector3 pos, Actor actor)
        {
            var block = GetBlock(pos);
            if (block == null)
                return false;
            return block.CheckActorOnBlock(actor);
        }
        
        public bool IsStayable(Vector3 pos)
        {
            if (!_mapDict.ContainsKey(pos))
                return false;
            return IsStayable(_mapDict[pos]);
        }
        public bool IsStayable(Block tile)
        {
            if(tile == null)
                return false;
            if(tile.isWalkable == false)
                return false;
            if (tile.IsActorOnBlock == true)
                return false;

            return true;
        }
    }
}