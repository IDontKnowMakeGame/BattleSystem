using System.Collections.Generic;
using System.Linq;
using Actors.Bases;
using Actors.Characters;
using Blocks;
using Managements;
using Managements.Managers;
using UnityEngine;
using Actors.Characters.Player;
using AttackDecals;
using Blocks.Acts;
using Cinemachine;

namespace Core
{
    public static class Define
    {
        public enum Sound
        {
            Bgm,
            Effect,
            MaxCount,
        }

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

        private static CinemachineVirtualCamera virtualCamera = null;
        public static CinemachineVirtualCamera VirtualCamera
        {
            get
            {
                if(virtualCamera == null)
                    virtualCamera = GameObject.Find("PlayerCam").GetComponent<CinemachineVirtualCamera>();

                return virtualCamera;
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
            var map = Define.GetManager<MapManager>();
            if (map == null)
                return null;
            var block = map?.GetBlock(position);
            if (block == null)
            {
                return null;
            }
            return block;
        }

        public static Actor GetActor(Vector3 pos) => GetBlock(pos) ? GetBlock(pos).ActorOnBlock : null;
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

        public static void Attack(Vector3 pos, Vector3 size, float damage, float delay, CharacterActor attacker, bool isLast = false)
        {
            var block = GetBlock(pos.SetY(0));
            if (block == null)
            {
                if (isLast)
                {
                    var state = CharacterState.Hold | CharacterState.Attack;
                    attacker.RemoveState(state);
                }
                return;
            }
            var resourceManager = Define.GetManager<ResourceManager>();
            var decalObj = resourceManager.Instantiate("AttackDecal");
            decalObj.transform.position = pos.SetY(0f);
            decalObj.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            decalObj.transform.SetParent(block.transform.GetChild(0));
            var rect = new Rect(new Vector2(pos.x - size.x / 2, pos.z - size.z / 2), new Vector2(size.x, size.z));
            

            var decal = decalObj.GetComponent<AttackDecal>();
            decal.Attack(rect, attacker, damage, delay, isLast);
        }

        public static void ShakeBlock(Vector3 pos, float duration, MovementType type)
        {
            var block = GetBlock(pos);

            block?.Shake(duration, type);
        }


    }

}