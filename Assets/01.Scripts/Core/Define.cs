using Managements;
using Managements.Managers;
using Managements.Managers.Base;
using Units.Base.Player;
using Units.Base.Unit;
using UnityEngine;

namespace Core
{
    public static class Define
    {
        public static T GetManager<T>() where T : Manager, new()
        {
            var manager = GameManagement.Instance.GetManager<T>() ?? GameManagement.Instance.AddManager<T>();
            return manager;
        }

        public static Camera MainCam => Camera.main;
    }

    public static class InGame
    {
        private static PlayerBase _playerBase = null;
        private static EnemyBase _bossBase = null;
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

        public static EnemyBase BossBase
        {
            get => _bossBase;
            set => _bossBase = value;
        }

        public static Units.Base.Units GetUnit(Vector3 pos)
        {
            var block = Define.GetManager<MapManager>().GetBlock(pos);
            return block == null ? null : block.GetUnit();
        }
        
        public static void SetUnit(UnitBase unit, Vector3 pos)
        {
            var block = Define.GetManager<MapManager>().GetBlock(pos);
            block.UnitOnBlock(unit);
        }
    }
}