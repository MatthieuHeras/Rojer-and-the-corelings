using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed;
    public float damage;
    public string targetTag;
    public GameObject deathEffect;

    private Vector3 movement;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D missing on projectile : " + gameObject.name);
            Destroy(gameObject);
        }
        Destroy(gameObject, 10f);
        movement = Vector3.right * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            Unit unit = collision.GetComponent<Unit>();
            if (unit != null)
                Hit(unit);
            Die();
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Obstacle obstacle = collision.GetComponent<Obstacle>();
            if (obstacle != null)
                Hit(obstacle);
            Die();
        }
    }

    protected virtual void Hit(Unit unit)
    {
        unit.TakeDamage(damage, true);
    }
    protected virtual void Hit(Obstacle obstacle)
    {
        obstacle.TakeDamage(damage);
    }
    private void Update()
    {
        transform.Translate(movement * Time.deltaTime);
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
