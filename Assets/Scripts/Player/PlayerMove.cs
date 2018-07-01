using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IControlSchemeHolder {

    public KeyCode up;
    public KeyCode left;
    public KeyCode down;
    public KeyCode right;
    public KeyCode popControls;
    public float speed;

    Rigidbody2D rb2d;
    UserControlScheme playerMove;

    public UserControlScheme GetControlScheme()
    {
        rb2d = GetComponent<Rigidbody2D>();

        EarlyUpdate.call += () =>
        {
           rb2d.velocity = Vector2.zero;
        };

        Dictionary<Func<bool>, Action> playerMoveControls = new Dictionary<Func<bool>, Action>();

        playerMoveControls[ () =>
        {
            //when this
            return Input.GetKey(up);
        }] = () =>
        {
            //this will trigger
            MovePlayer(new Vector2(0, 1f));
        };

        playerMoveControls[() =>
        {
            //when this
            return Input.GetKey(left);
        }
        ] = () =>
        {
            //this will trigger
            MovePlayer(new Vector2(-1f, 0));

        };

        playerMoveControls[() =>
        {
            //when this
            return Input.GetKey(down);
        }
        ] = () =>
        {
            //this will trigger
            MovePlayer(new Vector2(0, -1f));

        };

        playerMoveControls[() =>
        {
            //when this
            return Input.GetKey(right);
        }
        ] = () =>
        {
            //this will trigger
            MovePlayer(new Vector2(1f, 0));

        };

        playerMoveControls[() =>
        {
            return Input.GetKeyUp(popControls);
        }
        ] = () =>
        {
            UserControlManager.Instance.SetToDefault();
        };

        Action enterPlayerMove = () =>
        {
            Camera.main.GetComponent<CameraFollow>().Target = transform;
        };

        Action exitPlayerMove = () =>
        {
            Camera.main.GetComponent<CameraFollow>().Target = null;
        };

        return new UserControlScheme(playerMoveControls, enterPlayerMove, exitPlayerMove);
    }

    void MovePlayer(Vector2 direction)
    {
        rb2d.velocity += direction * speed * Time.deltaTime;
    }
}
