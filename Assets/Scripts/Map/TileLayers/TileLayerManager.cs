using System;
using System.Collections.Generic;
using UnityEngine;

public class TileLayerManager : MonoBehaviour
{

    public static TileLayerManager Instance { get; private set; }

    public enum LayerKey
    { 
        Floor,
        Furniture,
        Wall
    }

    private Dictionary<LayerKey, TileLayer> layers;

    //Foreach value in your list, the tile you are trying to place on must not belong to this layer
    private Dictionary<LayerKey, LayerKey[]> mutExLayers = new Dictionary<LayerKey, LayerKey[]>()
    {
        { LayerKey.Wall, new LayerKey[] { LayerKey.Furniture } },
        { LayerKey.Furniture, new LayerKey[] { LayerKey.Wall } },
        { LayerKey.Floor, new LayerKey[] { LayerKey.Wall } },
    };

    //Foreach value in your list, the tile you are trying to place on must belong to this layer
    private Dictionary<LayerKey, LayerKey[]> prerequisteLayers = new Dictionary<LayerKey, LayerKey[]>()
    {
        { LayerKey.Furniture, new LayerKey[] { LayerKey.Floor } },
    };

    private void Start()
    {
        Instance = this;

        layers = new Dictionary<LayerKey, TileLayer>();

        foreach(LayerKey k in Enum.GetValues(typeof(LayerKey)))
        {
            //Create canPlace using mutEx and pre-req layers
            Func<Tile, bool> canPlace = (t) => { return true; };

            if (mutExLayers.ContainsKey(k))
            {
                foreach (LayerKey mutEx in mutExLayers[k])
                {
                    canPlace += (t) =>
                    {
                        return this.layers[mutEx].ContainsTile(t) == false;
                    };
                }
            }


            if (prerequisteLayers.ContainsKey(k))
            {
                foreach (LayerKey prereq in prerequisteLayers[k])
                {
                    canPlace += (t) =>
                    {
                        return this.layers[prereq].ContainsTile(t);
                    };
                }
            }

            this.layers[k] = new ObjectLayer(k, canPlace);

            //Hierarchy management
            GameObject parent = new GameObject();

            parent.name = k.ToString();

            parent.transform.parent = this.transform;
        }
    }

    public TileLayer GetLayer(LayerKey layer)
    {
        return this.layers[layer];
    }

    public TileLayer GetLayer(string layerName)
    {
        TileLayerManager.LayerKey layerKey = (TileLayerManager.LayerKey)Enum.Parse(typeof(TileLayerManager.LayerKey), layerName);
        return TileLayerManager.Instance.GetLayer(layerKey);
    }

    public Transform GetLayerParent(LayerKey layer)
    {
        return transform.Find(layer.ToString());
    }
}

public class TileLayer
{
    protected List<Tile> layer;
    protected TileLayerManager.LayerKey layerKey;

    private Func<Tile, bool> canPlace;

    public TileLayer(TileLayerManager.LayerKey layerKey, Func<Tile, bool> canPlace)
    {
        this.layerKey = layerKey;
        this.layer = new List<Tile>();
        this.canPlace = canPlace;
    }

    public void AddTile(Tile t)
    {
        this.layer.Add(t);
    }

    public bool ContainsTile(Tile t)
    {
        return this.layer.Contains(t);
    }
    
    public virtual bool CanAddTile(Tile t)
    {
        //Canot add a tile which is already in the layer
        return ContainsTile(t) == false && canPlace(t);
    }

    public virtual void RemoveTile(Tile t)
    {
        this.layer.Remove(t);
    }
}
