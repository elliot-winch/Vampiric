using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]
public class Item : Damageable {

    [HideInInspector]
    public VillagerActions Reserving { get; set; }

    public GameObject representationPrefab;
    protected GameObject representationObj;

    [SerializeField]
    public List<Resource> resources;

    public Item(GameObject representationPrefab, List<Resource> resources, float maxHealth, float startingHealth = 0) : base(maxHealth, startingHealth)
    {
        this.representationPrefab = representationPrefab;
        this.resources = resources;
    }

    //Resource Management - I dont like this
    public Resource? GetResourceFromType(ResourceType type)
    {
        foreach (Resource r in resources)
        {
            if (r.type == type)
            {
                return r;
            }
        }

        return null;
    }
    
    //Inv Management
    public virtual void OnPlace(Tile t)
    {

    }

    public virtual void OnPickUp()
    {
        MonoBehaviour.Destroy(representationObj);
    }
    //end inv management

    //Health overrides
    public override void Destroyed()
    {
        base.Destroyed();

        MonoBehaviour.Destroy(representationObj);
    }

    //Villager selection - pick up this item
    public void VillagerInteraction(VillagerActions villager, Tile location)
    {
        //A reference to this item is found in the WorldItemManager
        WorldItemManager.Instance.RemoveItemFromWorldAt(location);

        OnPickUp();

        villager.Inv.Add(this);
    }
}*/

/*
 * Things items are made of
 */ 
[System.Serializable]
public enum Resource
{
    Wood,
    Dirt
}

[System.Serializable]
public struct ResourceRequirement
{
    public Resource Resource;
    public int Ammount;
}

[System.Serializable]
public class Item : Damageable
{
    public string Name;
    public List<ResourceRequirement> ResourceReqs;

    public Item(string name, List<ResourceRequirement> reqs, float maxHealth, float? startingHealth = null) : base(maxHealth, startingHealth)
    {
        this.Name = name;
        this.ResourceReqs = reqs;
    }
}


