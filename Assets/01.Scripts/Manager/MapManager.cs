using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Unit;
using Unit.Block;
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

    public void GiveDamage(Vector3 position, float damage, float delay)
    {
        instance.StartCoroutine(DamageCoroutine(position, damage, delay));
    }
    
    private IEnumerator DamageCoroutine(Vector3 position, float damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        position.y = 0;
        var block = GetBlock(position);
        Debug.Log(position);
        if (block != null)
        {
            var unit = block.GetUnit();
            if (unit != null)
                unit.GetBehaviour<UnitStat>().Damaged(damage);
        }
    }
}
