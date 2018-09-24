using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AStarPath
{
    public List<TileConnection> ValidPath { get; private set; }

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
                //path is complete
                ValidPath = ReconstructPath(path, startTile, endTile);
                return;
            }

            closedSet.Add(current);


            foreach (TileConnection connection in current.Connections)
            {

                if (connection.MoveCost == Mathf.Infinity)
                {
                    continue;
                }

                Tile neighbour = connection.Destination;

                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                float tentative_g_score = g_score[current] + connection.MoveCost;

                if (openSet.Contains(neighbour))
                {
                    //a shorter path has been found before
                    if (tentative_g_score >= g_score[neighbour])
                    {
                        continue;
                    } 
                }

                path[connection.Destination] = current;

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

    List<TileConnection> ReconstructPath(Dictionary<Tile, Tile> path, Tile start, Tile end)
    {

        List<TileConnection> validPath = new List<TileConnection>();
        Stack<Tile> pathStack = new Stack<Tile>();

        Tile current = end;

        pathStack.Push(end);

        while (path.ContainsKey(current))
        {
            current = path[current];
            pathStack.Push(current);
        }        

        current = pathStack.Pop();

        while (pathStack.Count > 0)
        {
            Tile prev = pathStack.Pop();

            Debug.Log(prev.Position);

            TileConnection tc = current.Connections.Single((compData) =>
            {
                return compData.Destination == prev;
            });

            validPath.Add(tc);

            current = prev;
        }

        

        return validPath;
    }

}