using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tile {

    public Vector2Int Position { get; private set; }
    [SerializeField]
    public TileType TileType;
    [SerializeField]
    public List<TileConnection> Connections { get; private set; }

    public Tile(Vector2Int position, TileType type)
    {
        this.Position = position;
        this.TileType = type;
        this.Connections = new List<TileConnection>();
    }

    public override bool Equals(object obj)
    {
        if(obj is Tile == false)
        {
            return false;
        }

        return ((Tile)obj).Position.Equals(Position);
    }
}

public class TileConnection
{
    public Tile Destination;
    public float MoveCost;
}


[Serializable]
//Represents a material a tile might be made of, e.g. sand, mud, grass, snow
public class TileType
{
    public string Name;
}
