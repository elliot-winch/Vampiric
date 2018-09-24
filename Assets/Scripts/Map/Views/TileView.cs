using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * How a tile is displayed in the scene
 * 
 */ 
public class TileView : MonoBehaviour {

    public void Display(Tile t, int tileResolution)
    {

        //Temp!
        Texture2D plainWhiteTexture = new Texture2D(tileResolution, tileResolution);

        for (int i = 0; i < tileResolution; i++)
        {
            for (int j = 0; j < tileResolution; j++)
            {
                Color c = Color.white;

                if (i < 1 || i >= tileResolution - 1 || j < 1 || j >= tileResolution - 1)
                {
                    c = Color.black;
                }

                plainWhiteTexture.SetPixel(i, j, c);
            }
        }
        plainWhiteTexture.filterMode = FilterMode.Point;
        plainWhiteTexture.Apply();
        //end temp

        gameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(plainWhiteTexture, new Rect(Vector2.zero, Vector2.one * tileResolution), Vector2.one * 0.5f);

        gameObject.transform.position = new Vector3(t.Position.x, t.Position.y);
    }
}
