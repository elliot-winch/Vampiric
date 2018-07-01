using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserControlScheme {

    private Dictionary<Func<bool>, Action> controls;
    Action enter;
    Action leave;

    public Dictionary<Func<bool>, Action> Controls
    {
        get
        {
            return controls;
        }
    }

    public Action Enter
    {
        get
        {
            return enter;
        }
    }

    public Action Leave
    {
        get
        {
            return leave;
        }
    }

    public UserControlScheme() : this(new Dictionary<Func<bool>, Action>())
    {
        
    }

    public UserControlScheme(Dictionary<Func<bool>, Action> scheme, Action enter = null, Action leave = null)
    {
        this.controls = scheme;
        this.enter = enter;
        this.leave = leave;
    }

    public void Merge(UserControlScheme ucs)
    {
        foreach(KeyValuePair<Func<bool>, Action> k in ucs.controls)
        {
            controls[k.Key] = k.Value;
        }

        this.enter += ucs.enter;
        this.leave += ucs.leave;
    }
}
