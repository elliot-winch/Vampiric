using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damageable {

    public float maxHealth;
    public float currentHealth;

    public Damageable(float maxHealth, float? startingHealth = null)
    {
        this.maxHealth = maxHealth;

        this.currentHealth = startingHealth ?? maxHealth;
    }
}
