using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour  {

    //Publics
    public GameObject dragValidPlacementUI;
    public PlaceableData[] items;

    private void Start()
    {
        foreach(PlaceableData pd in items)
        {
            if(pd.mode == BuildMode.DRAG && pd.relativeTiles.Length > 0)
            {
                Debug.LogWarning("ConstructionManager Warning!: Cannot drag mutli-tiled objects (yet). Either set this to another build mode, or remove unnecessary relative tiles");
            }
        }
    }

    public void BuildButtonPressed(string buttonID)
    {
        PlaceableData? selected = null;

        foreach(PlaceableData i in items)
        {
            if(i.buttonID == buttonID)
            {
                selected = i;
                break; 
            }
        }

        if (selected.HasValue)
        {
            GameObject placeable = selected.Value.prefab;

            UserControlScheme ucs = null;

            Func<Tile, bool> canPerform = (t) =>
            {
                return CanAddToLayer(t, TileLayerManager.Instance.GetLayer(selected.Value.layer), selected.Value);
            };

            Action<Tile> perform = (t) =>
            {
                AddToLayer(t, TileLayerManager.Instance.GetLayer(selected.Value.layer), selected.Value);
            };

            switch (selected.Value.mode)
            {
                case BuildMode.PLACE:
                    PlaceConstructionControls pcc = new PlaceConstructionControls();

                    ucs = pcc.BuildControlScheme(perform, canPerform, placeable);

                    break;
                case BuildMode.SINGLE_PLACE:
                    SinglePlacementConstructionControls scc = new SinglePlacementConstructionControls();

                    ucs = scc.BuildControlScheme(perform, canPerform, placeable);

                    break;
                case BuildMode.DRAG:
                    DragConstructionControls dcc = new DragConstructionControls();

                    ucs = dcc.BuildControlScheme(perform, canPerform, dragValidPlacementUI);

                    break;
                default:
                    return;
            }

            UserControlManager.Instance.SetControlScheme(ucs);
        }
    }

    //Helper for BuildButtonPressed
    private bool CanAddToLayer(Tile t, TileLayer l, PlaceableData obj)
    {
        List<Tile> tiles = AssembleTiles(t, obj.relativeTiles);

        //Determine validity - must be valid for all tiles
        foreach (Tile r in tiles)
        {
            if (l.CanAddTile(r) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void AddToLayer(Tile t, TileLayer l, PlaceableData obj)
    {
        List<Tile> tiles = AssembleTiles(t, obj.relativeTiles);

        //Add every tiles covered by the placeable to the layer
        foreach (Tile r in tiles)
        {
            l.AddTile(r);
        }

        //Spawn gameobject is needed
        if (l is ObjectLayer)
        {
            ((ObjectLayer)l).AddObject(obj.prefab, t, tiles);
        }
    }

    private List<Tile> AssembleTiles(Tile t, Vector2Int[] relativeTiles)
    {
        //Assemble all tiles
        List<Tile> tiles = new List<Tile>();

        tiles.Add(t);

        if (relativeTiles != null)
        {
            foreach (Vector2Int relativeCoord in relativeTiles)
            {
                tiles.Add(MapManager.Instance.GetTileAt(new Vector2Int(t.Position.x + relativeCoord.x, t.Position.y + relativeCoord.y)));
            }
        }

        return tiles;
    }

    public void RemoveButtonPressed(string buttonID)
    {
        TileLayer layer = TileLayerManager.Instance.GetLayer(buttonID);

        DragConstructionControls dcc = new DragConstructionControls();

        Func<Tile, bool> canPerform = (t) =>
        {
            return layer.ContainsTile(t);
        };

        UserControlScheme ucs = dcc.BuildControlScheme((t) =>
        {
            layer.RemoveTile(t);
        }, 
        canPerform,
        dragValidPlacementUI);

        UserControlManager.Instance.SetControlScheme(ucs);
    }
}

[System.Serializable]
public struct PlaceableData
{
    public string buttonID;
    public BuildMode mode;
    public TileLayerManager.LayerKey layer;
    public GameObject prefab;
    public Vector2Int[] relativeTiles;
}

public enum BuildMode
{
    PLACE,
    SINGLE_PLACE,
    DRAG
}