using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainSpear : SquadFighter
{
    public GameObject bullet;

    protected override void Start()
    {
        base.Start();
        if (bullet == null)
        {
            Debug.LogError("Bullet missing on unit : " + gameObject.name);
            Destroy(gameObject);
        }
    }
    protected override void Attack(GameObject target)
    {
        Look(target.transform.position - transform.position);
        GameObject newBullet = Instantiate(bullet, CustomLib.MoveInDirection(transform.position, target.transform.position, transform.localScale.x), transform.rotation);
        newBullet.transform.localScale.Scale(transform.localScale);
    }
}
