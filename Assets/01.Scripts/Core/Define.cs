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
            if(GameManagement.Instance == null)
                return null;
            var manager = GameManagement.Instance.GetManager<T>();
            return manager;
        }

        public static Camera MainCam => Camera.main;
    }

    public static class InGame
    {
        private static CameraMove _cameraMove = null;
        public static CameraMove CameraMove
        {
            get
            {
                if (_cameraMove == null)
                {
                    _cameraMove = Object.FindObjectOfType<CameraMove>();
                }

                return _cameraMove;
            }
        }
        
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
            set => _playerBase = value;
        }

        public static EnemyBase BossBase
        {
            get => _bossBase;
            set => _bossBase = value;
        }
        
        private static MeshParticle _meshParticle = null;
        
        public static MeshParticle MeshParticle
        {
            get
            {
                if (_meshParticle == null)
                {
                    _meshParticle = Object.FindObjectOfType<MeshParticle>();
                }

                return _meshParticle;
            }
            set => _meshParticle = value;
        }

        public static Units.Base.Units GetUnit(Vector3 pos)
        {
            pos.y = 0;
            var block = Define.GetManager<MapManager>().GetBlock(pos);
            return block == null ? null : block.GetUnit();
        }
        
        public static void SetUnit(UnitBase unit, Vector3 pos)
        {
            pos.y = 0;
            var block = Define.GetManager<MapManager>().GetBlock(pos);
            
            block.UnitOnBlock(unit);
        }
    }
}