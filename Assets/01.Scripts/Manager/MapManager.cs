using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unit;
using Unit.Block;
using Unit.Player;
using UnityEngine;

public class MapManager : IManager
{
    private Dictionary<Vector3, BlockBase> _map = new Dictionary<Vector3, BlockBase>();
    
    public void AddBlock(BlockBase block)
    {
        var position = block.transform.position;
        position.y = 0;
        _map.Add(position, block);
    }
    
    public BlockBase GetBlock(Vector3 position)
    {
        position.y = 0;
        _map.TryGetValue(position, out var block);
        return block;
    }

    public override void Awake()
    {
        var blocks = GameObject.FindObjectsOfType<BlockBase>().ToList();
        blocks.ForEach(AddBlock);
    }

    public void GiveDamage<T>(Vector3 position, float damage, float delay) where T : UnitStat
    {
        instance.StartCoroutine(DamageCoroutine<T>(position, damage, delay));
    }

    public bool BlockInUnit(Vector3 position)
    {
        position.y = 0;
        var block = GetBlock(position);
        if (block != null)
        {
            var unit = block.GetUnit();
            if (unit != null)
                return true;
        }
        return false;
    }

    public bool IsMovablePosition(Vector3 position)
    {
        position.y = 0;
        if(_map.ContainsKey(position) == false)
            return false;
        bool result = !BlockInUnit(position);
        return result;
    }
    
    private IEnumerator DamageCoroutine<T>(Vector3 position, float damage, float delay) where T : UnitStat
    {
        position.y = 0;
        var render = _map[position].GetBehaviour<BlockRender>(); 
        render.SetOutlineColor(Color.red);
        yield return new WaitForSeconds(delay);
        render.SetOutlineColor(Color.black);
        
        
        if (BlockInUnit(position))
        {
            var block = GetBlock(position);
            var unit = block.GetUnit();
            var stat = unit.GetBehaviour<T>();
            if (stat == null)
                yield break;
            if(stat.GetType() == typeof(T))
                stat.Damaged(damage);
            else if(stat.GetType().BaseType == typeof(T))
                stat.Damaged(damage);
        }
    }

    public List<BlockBase> GetNeighbors(BlockBase tile)
    {
        List<BlockBase> neighbors = new List<BlockBase>();
        int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
        bool[] walkableUDLR = new bool[4];

        for(int i = 0; i < 4; i++)
        {
            int checkX = tile.X + temp[i, 0];
            int checkZ = tile.Z + temp[i, 1];

            Vector3 checkPos = new Vector3(checkX, 0, checkZ);

            if (_map.ContainsKey(checkPos))
            {
                //if (_map[checkPos].isWalkable)
                //   walkableUDLR[i] = true;

                neighbors.Add(_map[checkPos]);
            }
        }

        return neighbors;
    }
}
