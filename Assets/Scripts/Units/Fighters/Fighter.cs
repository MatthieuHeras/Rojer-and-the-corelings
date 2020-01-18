using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : Unit
{
    public float damage;
    public float range;
    public float attackCD;
    public float refreshSearchCD = 0.5f;

    private float refreshSearchTimer = 0f;
    private float attackTimer;
    public GameObject target;

    protected abstract void Attack(GameObject target);

    protected override void Start()
    {
        base.Start();
        attackTimer = attackCD;
    }
    protected override void Update()
    {
        base.Update();
        attackTimer += Time.deltaTime;

        if (refreshSearchTimer >= refreshSearchCD)
        {
            refreshSearchTimer = 0f;
            SearchTarget();
        }
        else if (!isOrderedToMove)
            refreshSearchTimer += Time.deltaTime;
        if (attackTimer >= attackCD)
        {
            if (target != null && (Vector3.Distance(transform.position, target.transform.position)) - target.transform.localScale.x / 2f <= range)
            {
                attackTimer = 0f;
                Attack(target);
            }
        }
        if (target != null)
        {
            isWatchingSomething = true;
            Look(target.transform.position - transform.position); // Implicit cast to Vector2
        }
        else
            isWatchingSomething = false;
    }

    public void LoseSight(GameObject unit)
    {
        if (target == unit)
        {
            SearchTarget();
            refreshSearchTimer = 0f;
        }
    }

    protected void SearchTarget()
    {
        List<GameObject> ennemyList;
        if (gameObject.CompareTag("Human"))
            ennemyList = fieldOfView.seenCorelings;
        else
            ennemyList = fieldOfView.seenHumans;
        float smallestDist = 1000f;
        GameObject newTarget = null;
        foreach (GameObject unit in ennemyList)
        {
            float dist = Vector3.Distance(unit.transform.position, transform.position) - unit.transform.localScale.x / 2f;
            if (dist < smallestDist)
            {
                smallestDist = dist;
                newTarget = unit;
            }
        }
        target = newTarget;
        RefreshTarget();
    }
    private void RefreshTarget()
    {
        if (target == null)
        {
            GoThere(transform.position);
            return;
        }
        Vector3 targetPosition = CustomLib.MoveInDirection(target.transform.position, transform.position, (target.transform.localScale.x / 2f) + range - 0.1f);
        GoThere(targetPosition);
    }

    
}
