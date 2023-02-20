using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

namespace Managements.Managers.Floor
{
    public interface IFloor
    {
        public int FloorNumber { get; set; }
        public Vector3 PlayerSpawnPos { get; set; }
        public Vector3 BossSpawnPos { get; set; }
        public List<Vector3> BossArea { get; set; }
    }
}