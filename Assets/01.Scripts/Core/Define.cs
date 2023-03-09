using Managements;
using Managements.Managers;
using UnityEngine;

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
    }
}