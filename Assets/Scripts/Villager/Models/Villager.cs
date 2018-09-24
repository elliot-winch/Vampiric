using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Villager : Damageable {

    public string Name;

    [SerializeField]
    public Vector2 position;

    [SerializeField]
    public Inventory Inv;

    public Villager(string name, float maxHealth, float? startingHealth = null) : base(maxHealth, startingHealth)
    {
        this.Name = name;
        Inv = new Inventory();
    }
}
