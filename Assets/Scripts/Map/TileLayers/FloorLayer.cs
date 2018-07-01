using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLayer : ObjectLayer {

    ObjectLayer placeablesLayer;

    public FloorLayer(TileLayerManager.LayerKey layerKey, ObjectLayer placeablesLayer) : base(layerKey)
    {
        this.placeablesLayer = placeablesLayer;
    }

    public override bool CanAddTile(Tile t)
    {
        return base.CanAddTile(t) && placeablesLayer.ContainsTile(t) == false;
    }

}
