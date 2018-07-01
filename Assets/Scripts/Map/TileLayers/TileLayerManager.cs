using System;
using System.Collections.Generic;
using UnityEngine;

public class TileLayerManager : MonoBehaviour
{

    public static TileLayerManager Instance { get; private set; }

    public enum LayerKey
    { 
        Floor,
        Placeable
    }

    private Dictionary<LayerKey, TileLayer> layers;


    private void Start()
    {
        Instance = this;

        layers = new Dictionary<LayerKey, TileLayer>();

        ObjectLayer placeablesLayer = new ObjectLayer(LayerKey.Placeable);

        layers[LayerKey.Placeable] = placeablesLayer;
        layers[LayerKey.Floor] = new FloorLayer(LayerKey.Floor, placeablesLayer);

        foreach(LayerKey k in layers.Keys)
        {
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

    public TileLayer(TileLayerManager.LayerKey layerKey)
    {
        this.layerKey = layerKey;
        this.layer = new List<Tile>();
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
        return ContainsTile(t) == false;
    }

    public virtual void RemoveTile(Tile t)
    {
        this.layer.Remove(t);
    }
}
