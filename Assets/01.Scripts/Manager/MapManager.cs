using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Unit;
using UnityEngine;

public class MapManager : IManager
{
    private Dictionary<Vector3, Block> _map = new Dictionary<Vector3, Block>();
    
    public void AddBlock(Block block)
    {
        var position = block.transform.position;
        position.y = 0;
        _map.Add(position, block);
    }
    
    public Block GetBlock(Vector3 position)
    {
        position.y = 0;
        _map.TryGetValue(position, out var block);
        return block;
    }

    public override void Awake()
    {
        var blocks = GameObject.FindObjectsOfType<Block>().ToList();
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
        if (block != null)
        {
            var unit = block.GetUnit();
            Debug.Log(unit);
            if (unit != null)
                unit.GetBehaviour<UnitStat>().Damaged(damage);
        }
    }
}
