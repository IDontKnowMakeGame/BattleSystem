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
    }
}