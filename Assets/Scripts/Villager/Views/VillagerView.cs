using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerView : MonoBehaviour {

    PathMovement pathMove;

    public void Init(Villager villager, Tile startTile)
    {
        Display(villager);

        pathMove = gameObject.AddComponent<PathMovement>();
        pathMove.Init(startTile);
        pathMove.moveSpeed = 1f; //temp
    }

	void Display(Villager villger)
    {
        //temp
        Texture2D vilTexture = new Texture2D(16, 16);

        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                Color c = Color.red;

                vilTexture.SetPixel(i, j, c);
            }
        }
        vilTexture.filterMode = FilterMode.Point;
        vilTexture.Apply();
        //end temp

        gameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(vilTexture, new Rect(Vector2.zero, Vector2.one * 16), Vector2.one * 0.5f);
    }
}
