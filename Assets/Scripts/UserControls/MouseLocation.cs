using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocation : MonoBehaviour {

    static Tile tileThisFrame;
    static RaycastHit2D[] hitsThisFrame;

    public static Tile TileThisFrame
    {
        get
        {
            return tileThisFrame;
        }
    }

    public static RaycastHit2D[] HitsThisFrame
    {
        get
        {
            return hitsThisFrame;
        }
    }

    void Start()
    {
        EarlyUpdate.call += () =>
        {

            hitsThisFrame = null;
            tileThisFrame = null;

            Vector2 mouseScreenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseScreenPoint, Vector2.zero);


            if(hits != null)
            {
                hitsThisFrame = hits;

                foreach (RaycastHit2D hit in hits)
                {

                    if (hit.transform.GetComponent<Tile>() != null)
                    {

                        tileThisFrame = hit.transform.GetComponent<Tile>();
                    }
                }
            } 
        };
    }
}
