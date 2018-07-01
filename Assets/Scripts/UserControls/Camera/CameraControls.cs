using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    private static CameraControls instance;

    public static CameraControls Instance
    {
        get
        {
            return instance;
        }
    }

    public KeyCode up;
    public KeyCode left;
    public KeyCode down;
    public KeyCode right;
    public float speed;

    private void Start()
    {
        instance = this;

        if(UserControlManager.Instance != null)
        {
            UserControlManager.Instance.SetToDefault();
        }
    }

    public UserControlScheme BuildControlScheme () {
        //Base controller
        Dictionary<Func<bool>, Action> cameraControls = new Dictionary<Func<bool>, Action>();

        cameraControls[() =>
        {
            //when this
            return Input.GetKey(up);
        }
        ] = () =>
        {
            //this will trigger
            Move(new Vector3(0, 1f));
        };

        cameraControls[() =>
        {
            //when this
            return Input.GetKey(left);
        }
        ] = () =>
        {
            //this will trigger
            Move(new Vector3(-1f, 0));

        };

        cameraControls[() =>
        {
            //when this
            return Input.GetKey(down);
        }
        ] = () =>
        {
            //this will trigger
            Move(new Vector3(0, -1f));

        };

        cameraControls[() =>
        {
            //when this
            return Input.GetKey(right);
        }
        ] = () =>
        {
            //this will trigger
            Move(new Vector3(1f, 0));

        };

        ISelectionChangesControls selectable = null;

        cameraControls[() =>
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(MouseLocation.HitsThisFrame != null)
                {
                    foreach (RaycastHit2D hit in MouseLocation.HitsThisFrame)
                    {
                        if (hit.transform.GetComponent<ISelectionChangesControls>() != null)
                        {
                            selectable = hit.transform.GetComponent<ISelectionChangesControls>();
                            return true;
                        }
                    }
                }

            }

            return false;
        }
        ] = () =>
        {
            UserControlManager.Instance.SetControlScheme(selectable.GetControlScheme());
        };

        return new UserControlScheme(cameraControls);
    }

    void Move(Vector3 direction)
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
