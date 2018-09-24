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

    public Grid(int mapWidth, int mapHeight, TileType[] tileTypes)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;

        this.grid = new Tile[mapWidth, mapHeight];

        Generate(mapWidth, mapHeight, tileTypes);
    }

    void Generate(int mapWidth, int mapHeight, TileType[] tileTypes)
    {
        //SPawn nodes
        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                //TODO: proper tile generation
                TileType type = tileTypes[(int)(Random.Range(0, 1) * tileTypes.Length)];

                this.grid[i, j] = new Tile(new Vector2Int(i, j), type);
            }
        }

        float sqrtTwo = Mathf.Sqrt(2f);

        //Spawn Edges
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Tile t = this.grid[i, j];

                //Left
                if(i > 0)
                {
                    t.Connections.Add(new TileConnection()
                    {
                        Destination = grid[i - 1, j],
                        MoveCost = 1f, //todo
                    });

                    //Down left
                    if (j > 0)
                    {
                        t.Connections.Add(new TileConnection()
                        {
                            Destination = grid[i - 1, j - 1],
                            MoveCost = 1f//sqrtTwo, //todo
                        });
                    }

                    //Up left
                    if(j < mapHeight - 1)
                    {
                        t.Connections.Add(new TileConnection()
                        {
                            Destination = grid[i - 1, j + 1],
                            MoveCost = 1f//sqrtTwo, //todo
                        });
                    }
                }

                //Right
                if (i < mapWidth - 1)
                {
                    t.Connections.Add(new TileConnection()
                    {
                        Destination = grid[i + 1, j],
                        MoveCost = 1f, //todo
                    });

                    //Down right
                    if (j > 0)
                    {
                        t.Connections.Add(new TileConnection()
                        {
                            Destination = grid[i + 1, j - 1],
                            MoveCost = 1f//sqrtTwo, //todo
                        });
                    }

                    //Up Right
                    if (j < mapHeight - 1)
                    {
                        t.Connections.Add(new TileConnection()
                        {
                            Destination = grid[i + 1, j + 1],
                            MoveCost = 1f//sqrtTwo, //todo
                        });
                    }
                }

                //Down
                if (j > 0)
                {
                    t.Connections.Add(new TileConnection()
                    {
                        Destination = grid[i, j - 1],
                        MoveCost = 1f, //todo
                    });
                }

                //Up
                if (j < mapHeight - 1)
                {
                    t.Connections.Add(new TileConnection()
                    {
                        Destination = grid[i, j + 1],
                        MoveCost = 1f, //todo
                    });
                }

                foreach (TileConnection tc in t.Connections)
                {
                    Debug.DrawLine(new Vector3(t.Position.x, t.Position.y), new Vector3(tc.Destination.Position.x, tc.Destination.Position.y), Color.red, 100f);
                }
            }
        }

    }

    #region Tile Retrival
    public Tile GetTileAt(int x, int y)
    {
        return this.grid[x, y];
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        return GetTileAt(pos.x, pos.y);
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
    #endregion

    public AStarPath GetPath(Tile from, Tile to)
    {
        return new AStarPath(this, from, to);
    }
}
