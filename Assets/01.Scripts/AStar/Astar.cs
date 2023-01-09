using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Manager;
using Core;

public class Astar : MonoBehaviour
{
    public Block start, end;
    public LineRenderer line;
    public bool isFinding = false;

    // Test Material
    public Material red;
    public Material blue;
    public Material green;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            start.GetComponent<Renderer>().material = blue;
            end.GetComponent<Renderer>().material = green;
            StartCoroutine("FindPath"); 
        }
    }
    IEnumerator FindPath()
    {
        bool pathSuccess = false;

        isFinding = true;

        List<Block> openList = new List<Block>();
        List<Block> closeList = new List<Block>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            // CurrentNode 탐색 -> openList 코스트 가장 작은 것 찾기
            Block currentTile = openList[0];
            for(int i = 1; i < openList.Count; i++)
            {
                if(openList[i].fCost + openList[i].h < currentTile.fCost + currentTile.h)
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
            foreach (Block tile in GameManagement.Instance.GetManager<MapManager>().GetNeighbors(currentTile))
            {
                if (!tile.isWalkable || closeList.Contains(tile))
                    continue;

                int nowCost = currentTile.g + GetDistance(currentTile, tile);
                if (nowCost < tile.g || !openList.Contains(tile))
                {
                    tile.g = nowCost;
                    tile.h = GetDistance(tile, end);
                    tile.parent = currentTile;

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
            RedPath(start, end);
        }

        yield return null;
    }

    void RedPath(Block startTile, Block endTile)
    {
        Block currentTile = endTile;
        while (currentTile != startTile)
        {
            if(currentTile != start && currentTile != end)
                currentTile.GetComponent<Renderer>().material = red;
            currentTile = currentTile.parent;
        }
    }

    Vector3[] RetracePath(Block startTile, Block endTile)
    {
        List<Block> path = new List<Block>();
        Block currentTile = endTile;
        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        Vector3[] waypoints = SimplefyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplefyPath(List<Block> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector3 oldDir = Vector3.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector3 newDir = new Vector3(path[i - 1].x - path[i].x, 0, path[i - 1].z - path[i].z);
            if (oldDir != newDir)
            {
                waypoints.Add(path[i - 1].tileOBJ.transform.position + Vector3.back * 0.1f);
            }
            oldDir = newDir;
        }
        waypoints.Add(start.tileOBJ.transform.position + Vector3.back * 0.1f);
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

    int GetDistance(Block tile1, Block tile2)
    {
        int destX = Mathf.Abs(tile1.x - tile2.x);
        int destY = Mathf.Abs(tile1.z - tile2.z);

        if (destX > destY)
            return 14 * destY + 10 * (destX - destY);

        return 14 * destX + 10 * (destY - destX);

    }
}
