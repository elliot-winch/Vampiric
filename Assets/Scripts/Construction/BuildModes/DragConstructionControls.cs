using System;
using System.Collections.Generic;
using UnityEngine;

public class DragConstructionControls : ConstructionControls {

    public UserControlScheme BuildControlScheme(Action<Tile> perform, Func<Tile, bool> canPerform, GameObject prefabs)
    {

        Dictionary<Func<bool>, Action> controlScheme = new Dictionary<Func<bool>, Action>();

        Tile startTile = null;
        Tile mostRecent = null;

        Dictionary<Tile, GameObject> uiPool = new Dictionary<Tile, GameObject>();

        //left click held
        controlScheme[() =>
        {
            return (Input.GetMouseButton(0));
        }
        ] = () =>
        {
            if (startTile == null)
            {
                startTile = MouseLocation.TileThisFrame;
            }

            mostRecent = MouseLocation.TileThisFrame != null ? MouseLocation.TileThisFrame : mostRecent;

            if (startTile == null && mostRecent == null)
            {
                return;
            }

            List<Tile> tiles = new List<Tile>();

            int minX = Mathf.Min(startTile.Position.x, mostRecent.Position.x);
            int maxX = Mathf.Max(startTile.Position.x, mostRecent.Position.x);
            int minY = Mathf.Min(startTile.Position.y, mostRecent.Position.y);
            int maxY = Mathf.Max(startTile.Position.y, mostRecent.Position.y);

            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    tiles.Add(MapManager.Instance.GetTileAt(new Vector2Int(i, j)));
                }
            }

            //Remove ui no longer in tile list
            Tile[] keys = new Tile[uiPool.Keys.Count];
            uiPool.Keys.CopyTo(keys, 0);

            foreach (Tile t in keys)
            {
                if (tiles.Contains(t) == false)
                {
                    MonoBehaviour.Destroy(uiPool[t]);

                    uiPool.Remove(t);
                }
            }

            foreach (Tile t in tiles)
            {
                //Add new tiles if they aren't already added
                if (canPerform(t) && uiPool.ContainsKey(t) == false)
                {
                    //TODO: cleaner parent
                    uiPool[t] = MonoBehaviour.Instantiate(prefabs, new Vector3(t.Position.x, t.Position.y, 0f), Quaternion.identity);
                }
                //If the tile is still in the set but now invalid
                else if (canPerform(t) == false && uiPool.ContainsKey(t))
                {
                    MonoBehaviour.Destroy(uiPool[t]);
                    uiPool.Remove(t);

                }
            }
        };

        //left click up
        controlScheme[() =>
        {
            return (Input.GetMouseButtonUp(0));
        }
        ] = () =>
        {
            if(startTile != null && mostRecent != null)
            {

                int minX = Mathf.Min(startTile.Position.x, mostRecent.Position.x);
                int maxX = Mathf.Max(startTile.Position.x, mostRecent.Position.x);
                int minY = Mathf.Min(startTile.Position.y, mostRecent.Position.y);
                int maxY = Mathf.Max(startTile.Position.y, mostRecent.Position.y);

                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        Tile t = MapManager.Instance.GetTileAt(new Vector2Int(i, j));
                        if (canPerform(t))
                        {
                            perform(t);
                        }
                    }
                }
            }

            //Reset UI
            foreach (GameObject ui in uiPool.Values)
            {
                MonoBehaviour.Destroy(ui);
            }
            uiPool = new Dictionary<Tile, GameObject>();

            startTile = null;
            mostRecent = null;
        };

        //Right click
        controlScheme[() =>
        {
            return (Input.GetMouseButtonDown(1));
        }
        ] = () =>
        {
            UserControlManager.Instance.SetToDefault();
        };

        Action leave = () =>
        {
            //Remove left over UI
            foreach (GameObject ui in uiPool.Values)
            {
                MonoBehaviour.Destroy(ui);
            }
        };

        UserControlScheme ucs = new UserControlScheme(controlScheme, null, leave);

        ucs.Merge(base.BuildControlScheme(perform));

        return ucs;
    }
}
