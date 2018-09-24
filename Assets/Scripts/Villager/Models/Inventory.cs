using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory {

    [SerializeField]
    public List<Item> items;

    public Inventory(List<Item> startingItems = null)
    {
        items = startingItems != null ? startingItems : new List<Item>();
    }
}
