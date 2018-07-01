using System;
using System.Collections.Generic;
using UnityEngine;

public class PlaceConstructionControls : ConstructionControls {

    protected string name;

    public virtual UserControlScheme BuildControlScheme(Action<Tile> perform, Func<Tile, bool> canPerform, GameObject previewObj)
    {

        Dictionary<Func<bool>, Action> controlScheme = new Dictionary<Func<bool>, Action>();

        //left click down
        controlScheme[() =>
        {
            return (Input.GetMouseButtonDown(0) && MouseLocation.TileThisFrame != null);
        }
        ] = () =>
        {
            if (canPerform(MouseLocation.TileThisFrame))
            {
                perform(MouseLocation.TileThisFrame);
            }
        };

        //Mouse follow
        GameObject mouseFollowObj = null;

        //update
        controlScheme[() =>
        {
            return MouseLocation.TileThisFrame != null && mouseFollowObj != null;
        }
        ] = () =>
        {
            if (mouseFollowObj != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseFollowObj.transform.position = new Vector3((int)mousePos.x, (int)mousePos.y, 0f);
            }
        };

        Action enterScheme = () =>
        {
            if (previewObj != null && previewObj.GetComponent<SpriteRenderer>() != null)
            {
                mouseFollowObj = new GameObject();

                mouseFollowObj.AddComponent<SpriteRenderer>().sprite = previewObj.GetComponent<SpriteRenderer>().sprite;

                mouseFollowObj.transform.localScale = previewObj.transform.localScale;
            }
        };

        Action exitScheme = () =>
        {
            if (mouseFollowObj)
            {
                MonoBehaviour.Destroy(mouseFollowObj);
            }
        };

        UserControlScheme ucs = new UserControlScheme(controlScheme, enterScheme, exitScheme);

        ucs.Merge(base.BuildControlScheme(perform));

        return ucs;
    }
}
