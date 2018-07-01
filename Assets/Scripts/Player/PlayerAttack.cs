using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IControlSchemeHolder {

    public KeyCode primaryAbilityKey = KeyCode.E;
    public KeyCode secondaryAbilityKey = KeyCode.Q;
    public KeyCode tertiaryAbilityKey = KeyCode.F;


    public UserControlScheme GetControlScheme()
    {
        Dictionary<Func<bool>, Action> playerAttackControls = new Dictionary<Func<bool>, Action>();

        playerAttackControls[() =>
        {
            //left click
            return (Input.GetMouseButtonDown(0));
        }
        ] = () =>
        {
            //slash
        };

        playerAttackControls[() =>
        {
            //right click
            return (Input.GetMouseButtonDown(1));
        }
        ] = () =>
        {
            //execute
        };

        playerAttackControls[() =>
        {
            return (Input.GetKeyDown(primaryAbilityKey));
        }
        ] = () =>
        {
            Debug.Log(primaryAbilityKey);
        };

        playerAttackControls[() =>
        {
            return (Input.GetKeyDown(secondaryAbilityKey));
        }
        ] = () =>
        {
            Debug.Log(secondaryAbilityKey);

        };

        playerAttackControls[() =>
        {
            return (Input.GetKeyDown(tertiaryAbilityKey));
        }
        ] = () =>
        {
            Debug.Log(tertiaryAbilityKey);

        };

        return new UserControlScheme(playerAttackControls);
    }
}
