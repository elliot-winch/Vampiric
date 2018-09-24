using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class VillagerFactory : MonoBehaviour {

    public static VillagerFactory Instance
    {
        get; private set;
    }

    List<PathMovement> villagerPaths; 

    //testing
    public int test_Num_Villagers = 1;

    private void Start()
    {
        Instance = this;

        villagerPaths = new List<PathMovement>();

        for(int i = 0; i < test_Num_Villagers; i++)
        {
            Villager v = new Villager("John", 100f);

            GameObject obj = new GameObject
            {
                name = v.Name,
            };

            obj.AddComponent<VillagerView>().Init(v, MapFactory.Instance.GetRandomTile());

            obj.GetComponent<PathMovement>().MoveTo(MapFactory.Instance.GetRandomTile());
        }
    }
    //end testing

    public void RecalculateAllPaths(Tile t)
    {
        foreach(PathMovement vm in villagerPaths)
        {
            //TODO: only move if obstructed

            vm.MoveTo(vm.DestinationTile);
        }
    }
}
