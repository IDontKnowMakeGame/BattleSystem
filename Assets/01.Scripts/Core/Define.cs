using Managements;
using Managements.Managers;
using UnityEngine;
using Actor.Bases;

namespace Core
{
    public static class Define
    {
        private static Camera _mainCamera;

        public static Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }


        public static T GetManager<T>() where T : Manager
        {
            return GameManagement.Instance.GetManager<T>();
        }
    }

    public static class InGame
    {
        private static PlayerController _player;
        public static PlayerController Player
        {
            get
            {
                if(_player == null)
                {
                    _player = Object.FindObjectOfType<PlayerController>();
                }

                return _player;
            }
        }

        public static void SetActor(Vector3 pos, ActorController actor)
        {
            pos.y = 0;
            var map = Define.GetManager<MapManager>();
            var curBlock = map.GetBlock(actor);
            var nextblock = map.GetBlock(pos);
            curBlock.SetActorOnBlock(null);
            nextblock.SetActorOnBlock(actor);
        }
    }
}