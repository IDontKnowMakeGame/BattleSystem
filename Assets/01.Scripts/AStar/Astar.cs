using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using Manager;
using Unit;
using Unit.Block;
using UnityEngine.UI;

public class Astar
{
    private Stack<BlockBase> route = new Stack<BlockBase>();
    private BlockBase start, end;
    public LineRenderer line;
    public bool isFinding = false;

    // Test Material
    public Material red;
    public Material blue;
    public Material green;

    public IEnumerator FindPath()
    {
        
        bool pathSuccess = false;

        isFinding = true;

        List<BlockBase> openList = new List<BlockBase>();
        List<BlockBase> closeList = new List<BlockBase>();

        openList.Add(start);
        while(openList.Count > 0)
        {
            // CurrentNode 탐색 -> openList 코스트 가장 작은 것 찾기
            BlockBase currentTile = openList[0];
            for(int i = 1; i < openList.Count; i++)
            {
                if(openList[i].fCost + openList[i].H < currentTile.fCost + currentTile.H)
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closeList.Add(currentTile);

            if(currentTile == end)
            {
                pathSuccess = true;
                break;
            }

            yield return new WaitUntil(() => isFinding);
            // 이웃된 타일 가져오기
            foreach (BlockBase tile in GameManagement.Instance.GetManager<MapManager>().GetNeighbors(currentTile))
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

        
        // 라인 그리기
        if(pathSuccess)
        {
            MakePath(start, end);
            isFinding = false;
        }

        yield return null;
    }

    void RedPath(BlockBase startTile, BlockBase endTile)
    {
        BlockBase currentTile = endTile;
        while (currentTile != startTile)
        {
            if(currentTile != start && currentTile != end)
                currentTile.GetComponent<Renderer>().material = red;
            currentTile = currentTile.Parent;
        }
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

    Vector3[] RetracePath(BlockBase startTile, BlockBase endTile)
    {
        List<BlockBase> path = new List<BlockBase>();
        BlockBase currentTile = endTile;
        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }
        Vector3[] waypoints = SimplefyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplefyPath(List<BlockBase> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 oldDir = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 newDir = new Vector3(path[i - 1].X - path[i].X, 0, path[i - 1].Z - path[i].Z);
            if (oldDir != newDir)
            {
                waypoints.Add(path[i - 1].TileOBJ.transform.position + Vector3.back * 0.1f);
            }
            oldDir = newDir;
        }
        waypoints.Add(start.TileOBJ.transform.position + Vector3.back * 0.1f);
        return waypoints.ToArray();
    }

    public void DrawingLine(Vector3[] waypoints)
    {
        line.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++)
        {
            line.SetPosition(i, waypoints[i]);
        }
    }

    int GetDistance(BlockBase tile1, BlockBase tile2)
    {
        int destX = Mathf.Abs(tile1.X - tile2.X);
        int destY = Mathf.Abs(tile1.Z - tile2.Z);

        if (destX > destY)
            return 14 * destY + 10 * (destX - destY);

        return 14 * destX + 10 * (destY - destX);

    }
    
    public void SetRoute(BlockBase start, BlockBase end)
    {
        this.start = start;
        this.end = end;
    }

    public BlockBase GetNextPath()
    {
        if(route.Count > 0)
            return route.Pop();
        return null;
    }
    
    public bool HasFound()
    {
        return !isFinding;
    }
}
