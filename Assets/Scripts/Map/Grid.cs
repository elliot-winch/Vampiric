using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    Tile[,] grid;

    private int mapWidth;
    private int mapHeight;

    public List<Tile> AllTiles
    {
        get
        {
            List<Tile> allTiles = new List<Tile>();

            foreach(Tile t in grid)
            {
                allTiles.Add(t);
            }

            return allTiles;
        }
    }

    public Grid(int mapWidth, int mapHeight, int tileResolution)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;

        this.grid = new Tile[mapWidth, mapHeight];
    }

    public void AddTile(int x, int y, Tile t)
    {
        this.grid[x, y] = t;
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        return this.grid[pos.x, pos.y];
    }

    public List<Tile> GetNeighbours(Vector2Int position)
    {
        List<Tile> neighbours = new List<Tile>();

        neighbours.AddRange(GetYNeigbours(new Vector2Int(position.x, position.y)));

        if (position.x - 1 >= 0)
        {
            neighbours.Add(grid[position.x - 1, position.y]);
            neighbours.AddRange(GetYNeigbours(new Vector2Int(position.x - 1, position.y)));
        }

        if (position.x + 1 < mapWidth)
        {
            neighbours.Add(grid[position.x + 1, position.y]);
            neighbours.AddRange(GetYNeigbours(new Vector2Int(position.x + 1, position.y)));
        }

        return neighbours;
    }

    List<Tile> GetYNeigbours(Vector2Int position)
    {
        List<Tile> neighbours = new List<Tile>();

        if (position.y - 1 >= 0)
        {
            neighbours.Add(grid[position.x, position.y - 1]);
        }

        if (position.y + 1 < mapHeight)
        {
            neighbours.Add(grid[position.x, position.y + 1]);
        }

        return neighbours;
    }
}
