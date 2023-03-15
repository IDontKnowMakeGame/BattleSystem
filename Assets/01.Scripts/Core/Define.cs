using System.Collections.Generic;
using Actors.Bases;
using Blocks;
using Managements;
using Managements.Managers;
using UnityEngine;

namespace Core
{
    public static class Define
    {
        private static Camera _mainCamera;
        public static Camera MainCamera => _mainCamera ??= Camera.main;
        
        public static T GetManager<T>() where T : Manager
        {
            return GameManagement.Instance.GetManager<T>();
        }
    }

    public static class InGame
    {
        private static Dictionary<int, Actor> Actors = new();
        
        public static void AddActor(Actor actor)
        {
            if (Actors.ContainsKey(actor.UUID))
            {
                Debug.LogError($"Actor {actor.UUID} is already in this Dict.");
                return;
            }
            Actors.Add(actor.UUID, actor);
        }
        
        public static void RemoveActor(Actor actor)
        {
            if (!Actors.ContainsKey(actor.UUID))
            {
                Debug.LogError($"Actor {actor.UUID} is not in this Dict.");
                return;
            }
            Actors.Remove(actor.UUID);
        }
        
        public static Actor GetActor(int uuid)
        {
            if (!Actors.ContainsKey(uuid))
            {
                Debug.LogError($"Actor {uuid} is not in this Dict.");
                return null;
            }
            return Actors[uuid];
        }
        
        public static void AddBlock(Block block)
        {
            var mapManager = Define.GetManager<MapManager>();
            if (mapManager == null)
            {
                Debug.LogError($"MapManager is not in this Unit.");
                return;
            }
            mapManager.AddBlock(block.Position, block);
        }
        
        public static Block GetBlock(Vector3 position)
        {
            var block = Define.GetManager<MapManager>().GetBlock(position);
            if (block == null)
            {
                Debug.LogError($"Block {position} is not in this Map.");
                return null;
            }
            return block;
        }

        public static void SetActorOnBlock(Actor actor, Vector3 nextPos)
        {
            nextPos.y = 0;
            var currentBlock = GetBlock(actor.Position);
            var nextBlock = GetBlock(nextPos);
            
            if (currentBlock != null)
                currentBlock.RemoveActorOnBlock();
            if(nextBlock != null)
                nextBlock.SetActorOnBlock(actor);
        }
        
    }

}