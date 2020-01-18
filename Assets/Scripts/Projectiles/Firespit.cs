using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firespit : Projectile
{
    public float fireDamage;

    protected override void Hit(Unit unit)
    {
        base.Hit(unit);
        Ablaze ablaze = new Ablaze(fireDamage);
        unit.AddDebuff(ablaze);
    }
}
