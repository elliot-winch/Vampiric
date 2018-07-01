using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour  {

    //Publics
    public GameObject dragValidPlacementUI;
    public MenuItem[] items;

    public void BuildButtonPressed(string buttonID)
    {
        MenuItem? selected = null;

        foreach(MenuItem i in items)
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

            Action<Tile> perform = (t) =>
            {
                AddTileToLayer(t, TileLayerManager.Instance.GetLayer(selected.Value.layer), selected.Value.prefab);
            };

            switch (selected.Value.mode)
            {
                case BuildMode.PLACE:
                    PlaceConstructionControls pcc = new PlaceConstructionControls();

                    ucs = pcc.BuildControlScheme(perform, placeable);

                    break;
                case BuildMode.SINGLE_PLACE:
                    SinglePlacementConstructionControls scc = new SinglePlacementConstructionControls();

                    ucs = scc.BuildControlScheme(perform, placeable);

                    break;
                case BuildMode.DRAG:
                    DragConstructionControls dcc = new DragConstructionControls();

                    ucs = dcc.BuildControlScheme(perform, dragValidPlacementUI, TileLayerManager.Instance.GetLayer(selected.Value.layer));

                    break;
                default:
                    return;
            }

            UserControlManager.Instance.SetControlScheme(ucs);
        }
    }

    //Helper for BuildButtonPressed
    private void AddTileToLayer(Tile t, TileLayer l, GameObject obj)
    {

        if (l.CanAddTile(t))
        {
            l.AddTile(t);

            if (l is ObjectLayer)
            {
                ((ObjectLayer)l).AddObject(t, obj);
            }
        }
    }

    public void RemoveButtonPressed(string buttonID)
    {
        TileLayer layer = TileLayerManager.Instance.GetLayer(buttonID);

        DragConstructionControls dcc = new DragConstructionControls();

        UserControlScheme ucs = dcc.BuildControlScheme((t) =>
        {
            layer.RemoveTile(t);
        }, dragValidPlacementUI, layer);

        UserControlManager.Instance.SetControlScheme(ucs);
    }
}

[System.Serializable]
public struct MenuItem
{
    public string buttonID;
    public BuildMode mode;
    public TileLayerManager.LayerKey layer;
    public GameObject prefab;
}

public enum BuildMode
{
    PLACE,
    SINGLE_PLACE,
    DRAG
}