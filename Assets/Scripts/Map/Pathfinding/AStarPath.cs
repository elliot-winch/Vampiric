using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPath
{

    Stack<Tile> validPath;

    public AStarPath(Grid grid, Tile startTile, Tile endTile, bool TileValuesAffectPath = true)
    {

        List<Tile> closedSet = new List<Tile>();

        PriorityQueue<float, Tile> openSet = new PriorityQueue<float, Tile>();
        openSet.Enqueue(0, startTile);

        Dictionary<Tile, Tile> path = new Dictionary<Tile, Tile>();

        Dictionary<Tile, float> g_score = new Dictionary<Tile, float>();

        foreach (Tile h in grid.AllTiles)
        {
            g_score[h] = Mathf.Infinity;
        }

        g_score[startTile] = 0;

        Dictionary<Tile, float> f_score = new Dictionary<Tile, float>();

        foreach (Tile h in grid.AllTiles)
        {
            f_score[h] = Mathf.Infinity;
        }

        f_score[startTile] = heuristicCostEstimate(startTile, endTile);

        while (!openSet.IsEmpty)
        {
            Tile current = openSet.Dequeue().Value;

            if (current == endTile)
            {
                RecontructPath(path, current);
                return;
            }

            closedSet.Add(current);

            List<Tile> neighbours = grid.GetNeighbours(current.Position);

            foreach (Tile neighbour in neighbours)
            {

                if (MapManager.Instance.MoveCosts[neighbour] == Mathf.Infinity)
                {
                    continue;
                }

                if (closedSet.Contains(neighbour))
                {
                    continue;
                }


                float tentative_g_score = g_score[current] + (TileValuesAffectPath ? MapManager.Instance.MoveCosts[current] : 1f);

                if (openSet.Contains(neighbour) && tentative_g_score >= g_score[neighbour])
                {
                    continue;
                }

                path[neighbour] = current;
                g_score[neighbour] = tentative_g_score;
                f_score[neighbour] = g_score[neighbour] + heuristicCostEstimate(neighbour, endTile);

                if (openSet.Contains(neighbour) == false)
                {
                    openSet.Enqueue(f_score[neighbour], neighbour);
                }
            }
        }
    }

    float heuristicCostEstimate(Tile a, Tile b)
    {
        return Vector2.Distance(a.Position, b.Position);
    }

    void RecontructPath(Dictionary<Tile, Tile> path, Tile current)
    {

        validPath = new Stack<Tile>();
        Queue<Tile> pathQueue = new Queue<Tile>();

        pathQueue.Enqueue(current);

        while (path.ContainsKey(current))
        {
            current = path[current];
            pathQueue.Enqueue(current);
        }

        while (pathQueue.Count > 1)
        {
            validPath.Push(pathQueue.Dequeue());
        }
    }

    //public functions
    public Tile GetNextTile()
    {
        if (validPath.Count > 0)
        {
            return validPath.Pop();
        }
        else
        {
            return null;
        }
    }

    public bool IsNextTile()
    {
        return validPath != null && validPath.Count > 0;
    }

    public int Length()
    {
        return validPath.Count;
    }

    public bool Contains(Tile t)
    {
        return validPath.Contains(t);
    }
}