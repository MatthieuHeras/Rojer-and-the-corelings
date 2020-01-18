using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDemon : Demon
{
    public GameObject fireSpit;

    protected override void Attack(GameObject target)
    {
        Look(target.transform.position - transform.position);
        if (fireSpit != null)
            Instantiate(fireSpit, CustomLib.MoveInDirection(transform.position, target.transform.position, transform.localScale.x), transform.rotation);
    }
}
