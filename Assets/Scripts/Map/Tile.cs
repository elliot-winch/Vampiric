using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    TileType type;
    Vector2Int position;

    public TileType TileType
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    public Vector2Int Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }
}


//Represents a material a tile might be made of, e.g. sand, mud, grass, snow
public class TileType
{
    string name;
    float moveModifier;

    public TileType(string name, float moveModifier)
    {
        this.name = name;
        this.moveModifier = moveModifier;
    }

    public float MoveModifier
    {
        get
        {
            return moveModifier;
        }
    }
}
