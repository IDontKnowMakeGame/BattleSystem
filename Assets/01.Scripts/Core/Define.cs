using Managements;
using Managements.Managers.Base;
using Units.Base.Player;
using UnityEngine;

namespace Core
{
    public static class Define
    {
        private static PlayerBase _playerBase = null;

        public static PlayerBase PlayerBase
        {
            get
            {
                if (_playerBase == null)
                {
                    _playerBase = Object.FindObjectOfType<PlayerBase>();
                }

                return _playerBase;
            }
        }
        
        public static T GetManager<T>() where T : Manager, new()
        {
            var manager = GameManagement.Instance.GetManager<T>() ?? GameManagement.Instance.AddManager<T>();
            return manager;
        }
    }
}