using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff
{
    public float intensity;
    protected float decreaseSpeed;

    public Debuff()
    {
        intensity = 0f;
    }
    public Debuff(float intensity)
    {
        this.intensity = intensity;
    }

    public abstract bool Effect(Unit unit); // Return true if it still has effect
    public void AddIntensity(float addedIntensity)
    {
        intensity += addedIntensity;
    }
}
