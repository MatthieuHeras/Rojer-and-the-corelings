﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindDemon : Demon
{
    protected override void Attack(GameObject target)
    {
        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
            unit.Hit(damage);
    }
}
