using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    private static MapManager instance;

    public static MapManager Instance
    {
        get
        {
            return instance;
        }
    }

    public int mapHeight;
    public int mapWidth;
    public GameObject tilePrefab;
    public int tileResolution = 16;

    TileType[] tileTypes;

    Grid grid;
    Dictionary<Tile, float> moveCosts = new Dictionary<Tile, float>();

    public Dictionary<Tile, float> MoveCosts
    {
        get
        {
            return moveCosts;
        }
    }

    private void Start()
    {
        instance = this;

        tileTypes = new TileType[1];
        tileTypes[0] = new TileType("Grass", 1f);

        this.grid = new Grid(mapWidth, mapHeight, tileResolution);

        //Temp!
        Texture2D plainWhiteTexture = new Texture2D(tileResolution, tileResolution);

        for (int i = 0; i < tileResolution; i++)
        {
            for (int j = 0; j < tileResolution; j++)
            {
                Color c = Color.white;

                if (i < 1 || i >= tileResolution - 1 || j < 1 || j >= tileResolution - 1)
                {
                    c = Color.black;
                }

                plainWhiteTexture.SetPixel(i, j, c);
            }
        }
        plainWhiteTexture.filterMode = FilterMode.Point;
        plainWhiteTexture.Apply();
        //end temp


        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject tile_go = Instantiate(tilePrefab, new Vector2(i, j), Quaternion.identity, transform);

                tile_go.name = "Tile_" + i + "_" + j;

                Tile t = tile_go.GetComponent<Tile>();
                t.Position = new Vector2Int(i, j);
                t.TileType = tileTypes[0];

                this.grid.AddTile(i, j, t);

                tile_go.GetComponent<SpriteRenderer>().sprite = Sprite.Create(plainWhiteTexture, new Rect(0, 0, tileResolution, tileResolution), new Vector2(0f, 0f), tileResolution);

                moveCosts[t] = t.TileType.MoveModifier;
            }
        }
    }

    public AStarPath GetPath(Tile from, Tile to)
    {
        return new AStarPath(this.grid, from, to);
    }

    public Tile GetTileAt(Vector2Int pos)
    {
        return this.grid.GetTileAt(pos);
    }

    //test
    public Tile GetRandomTile()
    {
        return this.grid.GetTileAt(new Vector2Int(Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)));
    }
    //end
}
