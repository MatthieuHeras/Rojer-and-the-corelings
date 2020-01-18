using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ablaze : Debuff
{

    public Ablaze() : base()
    {
        decreaseSpeed = Const.ablazeDecreaseSpeed;
    }
    public Ablaze(float intensity) : base(intensity)
    {
        decreaseSpeed = Const.ablazeDecreaseSpeed;
    }

    public override bool Effect(Unit unit)
    {
        unit.TakeDamage(intensity * Time.deltaTime, false);
        intensity -= decreaseSpeed * Time.deltaTime;
        if (intensity <= 0f)
        {
            intensity = 0f;
            return false;
        }
        return true;
    }
}
