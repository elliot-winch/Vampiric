using System;
using System.Collections.Generic;
using UnityEngine;


public class ObjectLayer : TileLayer {

    Dictionary<Tile, GameObject> objs;

    public ObjectLayer(TileLayerManager.LayerKey layerKey, Func<Tile, bool> canPlace) : base(layerKey, canPlace)
    {
        this.objs = new Dictionary<Tile, GameObject>();
    }

    public virtual void AddObject(GameObject prefab, Tile sourceTile, List<Tile> includedTiless)
    {
        GameObject g = MonoBehaviour.Instantiate(prefab, new Vector3(sourceTile.Position.x, sourceTile.Position.y, 0f), Quaternion.identity);

        g.transform.parent = TileLayerManager.Instance.GetLayerParent(base.layerKey);

        foreach(Tile t in includedTiless)
        {
            objs[t] = g;
        }

        if (prefab.GetComponent<IConstructable>() != null)
        {
            prefab.GetComponent<IConstructable>().OnConstructed(sourceTile);
        }
    }


    public override void RemoveTile(Tile t)
    {
        if (objs.ContainsKey(t))
        {
            GameObject g = objs[t];

            Tile[] objKeys = new Tile[objs.Count];
            objs.Keys.CopyTo(objKeys, 0);

            foreach(Tile r in objKeys)
            {
                if(objs[r] == g)
                {
                    layer.Remove(r);
                    objs.Remove(r);
                }
            }
            
            MonoBehaviour.Destroy(g);
        }
    }
}
