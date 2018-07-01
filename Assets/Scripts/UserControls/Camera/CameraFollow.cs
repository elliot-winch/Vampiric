using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform target;

    public Transform Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;

        }
    }

    void Update () {
		
        if(target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
	}
}
