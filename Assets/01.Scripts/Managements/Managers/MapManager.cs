using System.Collections;
using System.Collections.Generic;
using Core;
using Managements.Managers.Base;
using Unit.Block;
using Units.Base.Player;
using Units.Base.Unit;
using Units.Behaviours.Unit;
using UnityEngine;

namespace Managements.Managers
{
    public class MapManager : Manager
    {
        private Dictionary<Vector3, BlockBase> _map = new();
        
        public void AddBlock(BlockBase block)
        {
            _map.Add(block.Position, block);
        }

        public BlockBase GetBlock(Vector3 pos)
        {
            pos.y = 0;
            return _map[pos];
        }

        public List<BlockBase> GetNeighbors(BlockBase tile)
        {
            List<BlockBase> neighbors = new List<BlockBase>();
            int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };

            for (int i = 0; i < 4; i++)
            {
                int checkX = tile.X + temp[i, 0];
                int checkZ = tile.Z + temp[i, 1];

                Vector3 checkPos = new Vector3(checkX, 0, checkZ);

                if (_map.ContainsKey(checkPos))
                {
                    neighbors.Add(_map[checkPos]);
                }
            }

            return neighbors;
        }

        public void Damage(Vector3 pos, float damage, float delay = 0.5f, Color color = new())
        {
            Instance.StartCoroutine(DamageBlockCoroutine(pos, damage, delay, color));
        }
        
        private bool DamageBlock(Vector3 pos, float damage)
        {
            var block = GetBlock(pos);
            if(block == null)
                return false;
            var target = block.GetUnit();
            if (target == null)
                return false;

            var targetStat = target.GetBehaviour<UnitStat>();
            targetStat.Damaged(damage);
            return true;
        }
        
        private IEnumerator DamageBlockCoroutine(Vector3 pos, float damage, float delay = 0.5f, Color color = new())
        {
            var block = GetBlock(pos);
            if (block == null)
                yield break;
            var render = block.GetBehaviour<BlockRender>();
            if (render == null)
                yield break;
            render.DOSetMainColor(color, delay);
            yield return new WaitForSeconds(delay);
            render.SetMainColor(Color.black);
            DamageBlock(pos, damage);
        }
    }
}