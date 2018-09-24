using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFactory : MonoBehaviour {

    public static MapFactory Instance { get; private set; }

    public int mapHeight;
    public int mapWidth;
    public int tileResolution = 16;

    public TileType[] tileTypes;

    public Grid MapGrid { get; private set; }

    private void Start()
    {
        Instance = this;

        this.MapGrid = new Grid(mapWidth, mapHeight, tileTypes);

        for(int i = 0; i < mapWidth; i++)
        {
            for(int j = 0; j < mapHeight; j++)
            {
                GameObject go = new GameObject
                {
                    name = "Tile",
                };

                go.transform.parent = transform;

                go.AddComponent<TileView>().Display(MapGrid.GetTileAt(i, j), tileResolution);
            }
        }

    }

    //test
    public Tile GetRandomTile()
    {
        return this.MapGrid.GetTileAt(new Vector2Int(Random.Range(0, mapWidth - 1), Random.Range(0, mapHeight - 1)));
    }
    //end
}
