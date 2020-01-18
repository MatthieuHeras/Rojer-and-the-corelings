using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musician : MonoBehaviour
{
    public float range = 10f;
    public float force = 5f;
    public GameObject deathEffect;
    public MusicEffect musicEffect;

    private float angle;
    private float coeff;
    private Camera cam;
    private FieldOfView fieldOfView;
    private Debuff currentDebuff;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        fieldOfView = GetComponentInChildren<FieldOfView>();
        if (cam == null)
        {
            Debug.LogError("Musician couldn't find camera : " + gameObject.name);
            Die();
        }
        if (fieldOfView == null)
        {
            Debug.LogError("Musician couldn't find FieldOfView : " + gameObject.name);
            Die();
        }
    }

    private void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.rotation = Quaternion.Euler(Vector3.forward * CustomLib.AngleFromPos(transform.position, mousePos));
        float dist = Vector2.Distance(transform.position, mousePos);

        if (dist <= 2f)
            dist = 0f;
        else if (dist >= 10)
            dist = Const.MaxAngle;
        else
            dist = Const.Dist2AngleA * dist + Const.Dist2AngleB;
        coeff = 2 * Mathf.PI / (2 * Mathf.PI - dist); // 1-8
        angle = 360f - (dist * Mathf.Rad2Deg);
        if (musicEffect != null)
            musicEffect.UpdateShape(angle);
        ApplyMusic(angle);
    }

    public void ApplyMusic(float angle)
    {
        currentDebuff = new Ablaze(force * Time.deltaTime * coeff);
        foreach(GameObject coreling in fieldOfView.seenCorelings)
        {
            Demon demon = coreling.GetComponent<Demon>();
            if (demon != null)
            {
                float angleFromMus = CustomLib.AngleFromPos(transform.position, demon.transform.position) - transform.rotation.eulerAngles.z;
                if (angleFromMus <= angle / 2f && angleFromMus >= -angle / 2f)
                {
                    demon.AddDebuff(currentDebuff);
                }
            }
        }
    }

    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
