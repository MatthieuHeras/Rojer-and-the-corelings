using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    public List<GameObject> seenHumans;
    public List<GameObject> seenCorelings;

    private CircleCollider2D coll;
    private void Start()
    {
        seenHumans = new List<GameObject>();
        seenCorelings = new List<GameObject>();
        coll = GetComponent<CircleCollider2D>();
        RefreshRadius();
    }
    private void OnTriggerEnter2D(Collider2D collision)  // When a unit enters the field of view, it's added to the corresponding list of seen units
    {
        if (collision.CompareTag("Human"))
            seenHumans.Add(collision.gameObject);
        else if (collision.CompareTag("Coreling"))
            seenCorelings.Add(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)  // When a unit exits the field of view, it's removed from the corresponding list of seen units
    {
        GameObject unit = collision.gameObject;
        if (unit == null)
            Debug.Log("unit is dead");
        if (TryRemoveUnit(unit))
        {
            Fighter parent = transform.parent.gameObject.GetComponent<Fighter>();
            if (parent != null)
                parent.LoseSight(unit);
        }
    }

    public List<GameObject> GetHumans()
    {
        return seenHumans;
    }
    public List<GameObject> GetCorelings()
    {
        return seenCorelings;
    }
    public bool TryRemoveUnit(GameObject unit)
    {
        if (seenHumans.Contains(unit))
        {
            seenHumans.Remove(unit);
            return true;
        }
        if (seenCorelings.Contains(unit))
        {
            seenCorelings.Remove(unit);
            return true;
        }
        return false;
    }
    public void RefreshRadius(float newRadius)
    {
        this.radius = newRadius;
        RefreshRadius();
    }

    private void RefreshRadius()
    {
        if (coll != null)
            coll.radius = this.radius;
    }
}
