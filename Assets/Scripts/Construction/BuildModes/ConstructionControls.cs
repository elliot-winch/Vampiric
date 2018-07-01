using System;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionControls {

    public virtual UserControlScheme BuildControlScheme(Action<Tile> perform)
    {

        Dictionary<Func<bool>, Action> controlScheme = new Dictionary<Func<bool>, Action>();

        controlScheme[() =>
        {
            //right click
            return (Input.GetMouseButtonDown(1));
        }
        ] = () =>
        {
            UserControlManager.Instance.SetToDefault();
        };

        UserControlScheme ucs = new UserControlScheme(controlScheme);

        ucs.Merge(CameraControls.Instance.BuildControlScheme());

        return ucs;
    }

}
