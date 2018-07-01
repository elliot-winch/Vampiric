using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlacementConstructionControls : PlaceConstructionControls {

    public override UserControlScheme BuildControlScheme(Action<Tile> perform, GameObject previewObj)
    {
        UserControlScheme ucs = base.BuildControlScheme(perform, previewObj);

        ucs.Controls[() =>
        {
            return Input.GetMouseButtonDown(0);
        }
        ] = () =>
        {
            UserControlManager.Instance.SetToDefault();
        };

        return ucs;
    }
}
