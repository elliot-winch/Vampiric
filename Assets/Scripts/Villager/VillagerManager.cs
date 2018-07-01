using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour {

    static VillagerManager instance;

    public static VillagerManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject villagerPrefab;

    List<VillageMove> allVillagers; //TODO replace with Villager class

    //testing
    int test_Num_Villagers = 1;

    private void Start()
    {
        instance = this;

        allVillagers = new List<VillageMove>();


        for(int i = 0; i < test_Num_Villagers; i++)
        {
            GameObject vill_go = Instantiate(villagerPrefab, transform);

            allVillagers.Add(vill_go.GetComponent<VillageMove>());

            vill_go.GetComponent<VillageMove>().Init(MapManager.Instance.GetTileAt(new Vector2Int(0,0)));

            Action onEndMoveToRandomTile = () =>
            {
                //vill_go.GetComponent<VillageMove>().MoveTo(MapManager.Instance.GetRandomTile());
            };

            vill_go.GetComponent<VillageMove>().MoveTo(MapManager.Instance.GetTileAt(new Vector2Int(9, 9)), onEndMoveToRandomTile);
        }
    }

    //end testing

    public void RecalculateAllPaths(Tile t)
    {
        foreach(VillageMove vm in allVillagers)
        {
            if(vm.CurrentTile == t)
            {
                //the tile they are currently stood in has become impassible
                vm.MoveTo(vm.DestinationTile);

            }



            //the path they are travelling on contains a tile which has become impassable
            if (vm.NextTile == t || (vm.CurrentPath != null && vm.CurrentPath.Contains(t)))
            {
                vm.MoveTo(vm.DestinationTile);
            }
        }
    }
}
