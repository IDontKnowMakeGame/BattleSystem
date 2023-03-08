using System.Collections.Generic;
using Units.Base.Default;
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
    }

    public static class InGame
    {
        private static Dictionary<int, Unit> _units = new();

        public static void AddUnit(int uuid, Unit unit)
        {
            _units.Add(uuid, unit);
        }
        
        public static Unit GetUnit(int uuid)
        {
            return _units[uuid];
        }
    }
}