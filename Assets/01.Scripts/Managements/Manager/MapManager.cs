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
    }
}