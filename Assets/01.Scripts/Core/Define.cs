using System.Collections.Generic;
using System.Linq;
using Actors.Bases;
using Actors.Characters;
using Blocks;
using Managements;
using Managements.Managers;
using UnityEngine;
using Actors.Characters.Player;

namespace Core
{
    public static class Define
    {
        private static Camera _mainCamera;

        public static Camera MainCamera
        {
            get => _mainCamera;
            set => _mainCamera = value;
        }
        
        public static T GetManager<T>() where T : Manager
        {
            if (GameManagement.Instance == null)
            {
                return null;
            }
            return GameManagement.Instance.GetManager<T>();
        }
    }

    public static class InGame
    {
        public static Dictionary<int, Actor> Actors { get; private set; } = new();

        private static Camera mainCam = null;
        public static Camera MainCam
        {
            get
            {
                if (mainCam == null)
                    mainCam = Camera.main;

                return mainCam;
            }
        }

        private static PlayerActor player = null;

        public static PlayerActor Player
        {
            get => player;
            set => player = value;
        }

        private static float none = -987654321f;

        public static float None => none;

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
                return null;
            }
            return block;
        }

        public static Actor GetActor(Vector3 pos) => GetBlock(pos).ActorOnBlock;

        public static CharacterActor[] GetNearCharacterActors(Vector3 pos)
        {
            var map = Define.GetManager<MapManager>();
            var block = map.GetBlock(pos);
            var blocks = map.GetNeighbors(block);
            var actors = from b in blocks select b.ActorOnBlock as CharacterActor; 
            return actors.ToArray();
        }

		public static void SetActorOnBlock(Actor actor)
        {
            //Debug.Log(actor.Position);
            var currentBlock = GetBlock(actor.Position);
            currentBlock.SetActorOnBlock(actor);
        }
        public static Vector3 CameraDir()
        {
            Vector3 cameraDir;

            Vector3 heading = InGame.MainCam.transform.localRotation * Vector3.forward;
            heading.y = 0;
            heading = heading.normalized;

            if (Mathf.Abs(heading.x) >= Mathf.Abs(heading.z))
            {
                if (heading.x >= 0) cameraDir = Vector3.right;
                else cameraDir = Vector3.left;
            }
            else
            {
                if (heading.z >= 0) cameraDir = Vector3.forward;
                else cameraDir = Vector3.back;
            }

            return cameraDir;
        }

        public static Vector3 CamDirCheck(Vector3 direction)
        {
            Vector3 cameraDir = CameraDir();

            if (cameraDir.x != 0)
            {
                float swap = direction.x;
                direction.x = direction.z * cameraDir.x;
                direction.z = cameraDir.x * -swap;
            }
            else if (cameraDir.z != 0)
            {
                direction.x = direction.x * cameraDir.z;
                direction.z = direction.z * cameraDir.z;
            }

            return direction;
        }
    }

}