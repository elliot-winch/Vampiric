using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class UserControlManager : MonoBehaviour {

    private static UserControlManager instance;

    public static UserControlManager Instance
    {
        get
        {
            return instance;
        }
    }

    UserControlScheme controlScheme;

    private void Start()
    {
        instance = this;

        if(CameraControls.Instance != null)
        {
            SetToDefault();
        }
    }

    private void Update()
    {
        if(controlScheme != null)
        {
            foreach (KeyValuePair<Func<bool>, Action> k in controlScheme.Controls)
            {
                if (k.Key())
                {
                    k.Value();
                }
            }
        }
    }

    public void SetControlScheme(UserControlScheme ucs)
    {
        if(controlScheme != null)
        {
            if(controlScheme.Leave != null)
            {
                controlScheme.Leave();
            }
        }

        controlScheme = ucs;

        if (ucs.Enter != null)
        {
            ucs.Enter();
        }
    }

    public void SetToDefault()
    {
        if (controlScheme != null)
        {
            if (controlScheme.Leave != null)
            {
                controlScheme.Leave();
            }
        }


        controlScheme = CameraControls.Instance.BuildControlScheme();
    }
}