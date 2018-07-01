using System.Collections.Generic;
using UnityEngine;


public class ObjectLayer : TileLayer {

    Dictionary<Tile, GameObject> objs;

    public ObjectLayer(TileLayerManager.LayerKey layerKey) : base(layerKey)
    {
        this.objs = new Dictionary<Tile, GameObject>();
    }

    public virtual void AddObject(Tile t, GameObject prefab)
    {
        GameObject g = MonoBehaviour.Instantiate(prefab, new Vector3(t.Position.x, t.Position.y, 0f), Quaternion.identity);

        g.transform.parent = TileLayerManager.Instance.GetLayerParent(base.layerKey);

        objs[t] = g;

        if (prefab.GetComponent<IConstructable>() != null)
        {

            prefab.GetComponent<IConstructable>().OnConstructed(t);
        }
    }


    public override void RemoveTile(Tile t)
    {
        base.RemoveTile(t);

        if (objs.ContainsKey(t))
        {
            GameObject g = objs[t];

            objs.Remove(t);

            MonoBehaviour.Destroy(g);
        }
    }
}
