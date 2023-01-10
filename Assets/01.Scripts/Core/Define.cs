using System.Collections;
using System.Collections.Generic;
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
                    _playerBase = GameObject.FindObjectOfType<PlayerBase>();
                }

                return _playerBase;
            }
        }
    }
}
