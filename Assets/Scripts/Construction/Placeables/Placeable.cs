using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour, IConstructable
{
    public float moveCost;

    //private Tile t; do we need this or can we use the manager

    public void OnConstructed(Tile t)
    {

        MapManager.Instance.MoveCosts[t] = moveCost;

        //TODO replace with "all pathfinders" manager
        VillagerManager.Instance.RecalculateAllPaths(t);
    }
}
