using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Managements;
using Managements.Managers;

public class Astar
{
    private Stack<BlockBase> route = new Stack<BlockBase>();
    private BlockBase start, end;
    public bool isFinding = false;

    public IEnumerator FindPath()
    {

        bool pathSuccess = false;

        isFinding = true;

        List<BlockBase> openList = new List<BlockBase>();
        List<BlockBase> closeList = new List<BlockBase>();

        openList.Add(start);
        while (openList.Count > 0)
        {
            // CurrentNode Research -> Find the smallest openList cost

            BlockBase currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost + openList[i].H < currentTile.fCost + currentTile.H)
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closeList.Add(currentTile);

            if (currentTile == end)
            {
                pathSuccess = true;
                break;
            }

            yield return new WaitUntil(() => isFinding);
            // Get Neighbored Tiles
            foreach (BlockBase tile in Define.GetManager<MapManager>().GetNeighbors(currentTile))
            {
                if (!tile.isWalkable || closeList.Contains(tile))
                    continue;
                int nowCost = currentTile.G + GetDistance(currentTile, tile);
                if (nowCost < tile.G || !openList.Contains(tile))
                {
                    tile.G = nowCost;
                    tile.H = GetDistance(tile, end);
                    tile.Parent = currentTile;

                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }
            }
        }


        // Line Draw
        if (pathSuccess)
        {
            MakePath(start, end);
            //RedPath(start, end);
            isFinding = false;
        }

        yield return null;
    }

    int GetDistance(BlockBase tile1, BlockBase tile2)
    {
        int destX = Mathf.Abs(tile1.X - tile2.X);
        int destY = Mathf.Abs(tile1.Z - tile2.Z);

        if (destX > destY)
            return 14 * destY + 10 * (destX - destY);

        return 14 * destX + 10 * (destY - destX);
    }

    public void SetPath(Vector3 startPos, Vector3 endPos)
    {
        var startTile = Define.GetManager<MapManager>().GetBlock(startPos);
        var endTile = Define.GetManager<MapManager>().GetBlock(endPos);
        SetPath(startTile, endTile);
    }
    
    public void SetPath(BlockBase startTile, BlockBase endTile)
    {
        start = startTile;
        end = endTile;
    }
        
    void MakePath(BlockBase startTile, BlockBase endTile)
    {
        route.Clear();
        BlockBase currentTile = endTile;
        while (currentTile != startTile)
        {
            if (currentTile != start)
            {
                route.Push(currentTile);
                currentTile = currentTile.Parent;
            }
        }
    }

    public BlockBase GetNextPath()
    {
        if(route.Count > 0)
            return route.Pop();
        return null;
    }

    public bool IsPathFining()
    {
        return isFinding == false;
    }
}
