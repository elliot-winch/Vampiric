using System;
using System.Collections.Generic;
using UnityEngine;

public class EarlyUpdate : MonoBehaviour {

    public static Action call;

    private void Update()
    {
        if(call != null)
        {
            call();
        }
    }
}
