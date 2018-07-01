using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllControls : MonoBehaviour, ISelectionChangesControls
{

    public UserControlScheme GetControlScheme()
    {
        UserControlScheme ucs = new UserControlScheme();

        foreach (IControlSchemeHolder u in GetComponents<IControlSchemeHolder>())
        {
            ucs.Merge(u.GetControlScheme());
        }

        return ucs;
    }
}
