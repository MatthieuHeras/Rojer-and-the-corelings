using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public float maxHealth;
    public float dodgeChance;
    public float speed;
    public float moral;
    public float armor;
    public float viewDist;
    public GameObject animationSpawn;
    public GameObject deathEffect;
    public GameObject dodgeEffect;
    public GameObject damageEffect;
    [System.NonSerialized] public Rigidbody2D rb;
    [System.NonSerialized] public AIPathfinder aiPathfinder;

    protected bool isWatchingSomething = false;
    [SerializeField] protected bool isOrderedToMove = false;
    protected FieldOfView fieldOfView;
    
    private static readonly float moveOrderCD = 1f;
    private float health;
    private float moveOrderTimer;
    private List<Debuff> debuffs = new List<Debuff>();
    private HealthBar healthBar;

    public void TakeDamage(float damage, bool doesHit)
    {
        damage *= (1 - (armor / 100f));
        if (damageEffect != null && doesHit)
        {
            if (animationSpawn != null)
                Instantiate(damageEffect, animationSpawn.transform);
        }
        health -= damage;
        if (health <= 0)
            Die();
        RefreshHealthBar();
    }
    public void Hit(float damage)
    {
        if (Random.value <= dodgeChance / 100f)
        {
            if (dodgeEffect != null)
            {
                GameObject effect = Instantiate(dodgeEffect);
                if (animationSpawn != null)
                    effect.transform.SetParent(animationSpawn.transform);
                effect.transform.localPosition = Vector3.zero;
                effect.transform.rotation = Quaternion.identity;
            }
                return;
        }
        TakeDamage(damage, true);
    }
    public virtual void GetAway(float dir) // The speaker asks the unit to move away
    {
        if (!isOrderedToMove)
        {
            isOrderedToMove = true;
            Vector3 targetPos = CustomLib.MoveInDirection(transform.position, dir, 0.5f);
            GoThere(targetPos);
        }
    }
    public void AddDebuff(Debuff debuff)
    {
        foreach (Debuff d in debuffs)
        {
            if (d.GetType().Equals(debuff.GetType()))
            {
                d.AddIntensity(debuff.intensity);
                return;
            }
        }
        debuffs.Add(debuff);
    }
    public void RemoveDebuff(Debuff debuff)
    {
        if (debuffs.Contains(debuff))
            debuffs.Remove(debuff);
    }

    private void Awake()
    {
        aiPathfinder = GetComponent<AIPathfinder>();
        healthBar = GetComponentInChildren<HealthBar>();
        fieldOfView = GetComponentInChildren<FieldOfView>();
        rb = GetComponent<Rigidbody2D>();
        if (maxHealth <= 0f || aiPathfinder == null || healthBar == null || fieldOfView == null || rb == null)
        {
            Debug.Log("Error during unit initialization : " + gameObject.name);
            Die();
        }
    }
    protected virtual void Start()
    {
        health = maxHealth;
        moveOrderTimer = moveOrderCD;
        fieldOfView.RefreshRadius(viewDist);
        aiPathfinder.speed = this.speed;
        RefreshHealthBar();
    }
    protected virtual void Update()
    {
        if (!isWatchingSomething && !(rb.velocity.Equals(Vector2.zero)))
            Look(rb.velocity);
        if (isOrderedToMove) // A unit can be told to move once per "moveOrderCD" second(s).
        {
            if (moveOrderTimer > moveOrderCD)
            {
                isOrderedToMove = false;
                moveOrderTimer = 0f;
            }
            else
                moveOrderTimer += Time.deltaTime;
        }
        else
            moveOrderTimer = 0f;

        ApplyDebuffs();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!(collision.collider.CompareTag(gameObject.tag))) // human collides with demon
            return;

        Unit unit = collision.collider.GetComponent<Unit>();
        if (unit == null) // Error
        {
            Debug.LogError("An object tagged as " + collision.collider.tag + " misses the unit script : " + collision.collider.name);
            return;
        }
        if (unit.aiPathfinder == null) // Error
        {
            Debug.LogError("A unit misses the AIPathfinder component : " + collision.collider.name);
            return;
        }

        float targetDir = CustomLib.AngleFromPos(Vector2.zero, unit.aiPathfinder.direction);
        float sourceDir = CustomLib.AngleFromPos(Vector2.zero, aiPathfinder.direction);

        // Get angles from directions and positions
        float angleTarget; 
        float angleSource;

        if (unit.aiPathfinder.direction.Equals(Vector2.zero))
            angleTarget = 0f;
        else
        {
            angleTarget = CustomLib.AngleFromPos(unit.rb.position, rb.position) - targetDir;
            if (angleTarget <= -180) // Transform angles from 0;360 to -180;180
                angleTarget += 360;
            else if (angleTarget > 180)
                angleTarget -= 360;
        }

        if (aiPathfinder.direction.Equals(Vector2.zero))
            angleSource = 0f;
        else
        {
            angleSource = CustomLib.AngleFromPos(rb.position, unit.rb.position) - sourceDir;
            if (angleSource <= -180)
                angleSource += 360;
            else if (angleSource > 180)
                angleSource -= 360;
        }

        if (Mathf.Abs(angleTarget) > Mathf.Abs(angleSource)) // The unit with the larger angle deal with the collision. The other awaits orders
            return;

        float angleIntensity = Mathf.Abs(angleSource);
        float dir = angleSource / angleIntensity; // 1 if left ; -1 if right
        float newDir = sourceDir - dir * (90f - angleIntensity);
        GetAway(Mathf.Deg2Rad * newDir); // source
        unit.GetAway(Mathf.Deg2Rad * (newDir + 180f)); // target   
    }

    protected void GoThere(Vector3 targetPosition)
    {
        aiPathfinder.UpdateTarget(targetPosition);
    }
    protected void Look(Vector2 lookDir)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(lookDir.y, lookDir.x));
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void RefreshHealthBar()
    {
        if (healthBar != null)
            healthBar.ChangeValue(health, maxHealth);
    }
    private void ApplyDebuffs()
    {
        for (int i = 0; i < debuffs.Count; i++)
        {
            if (!(debuffs[i].Effect(this)))
            {
                RemoveDebuff(debuffs[i]);
                i--;
            }
        }
    }
}
